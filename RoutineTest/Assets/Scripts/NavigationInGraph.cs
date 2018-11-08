using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#region 自定义图类型
public class Graph    //图
{
    private int Capacity;
    public Vertex[] Vertixes;  //全部节点
    public short[,] LinkMatrix; //邻接矩阵 
    int numVerts = 0;
    public Graph(int capacity)              //构造函数
    {
        Capacity = capacity;
        Vertixes = new Vertex[Capacity];
        LinkMatrix = new short[Capacity, Capacity];
        for (int i = 0; i < Capacity; i++)
        {
            for (int j = 0; j < Capacity; j++)
            {
                LinkMatrix[i, j] = 0;
            }
        }
    }

    public void AddVertex(Vertex v)       //添加点
    {
        if (numVerts == Capacity)
        {
            return;
        }
        Vertixes[numVerts] = v;
    }
    public void SortVertex()
    {
        Array.Sort((Vertixes), (ve1, ve2) => { return (ve1.ID.CompareTo(ve2.ID)); });
    }
    //删除不好写先放着

    public void Link(Vertex Vera,Vertex Verb)      //连接图上两点
    {
        if (Vera.ID < 0 || Verb.ID < 0)
        {
            return;
        }
        LinkMatrix[Vera.ID, Verb.ID] = 1;
    }
    public void Link(Vertex Vera, Vertex Verb, short Dis)
    {
        if (Vera.ID < 0 || Verb.ID < 0)
        {
            return;
        }
        LinkMatrix[Vera.ID, Verb.ID] = Dis;
    }
}

public class Vertex    //图上节点
{
    public bool DetectedAndShown = false;
    public int ID = -1;
    public int Depth=int.MaxValue;
    public GameObject Data;
    public List<Vertex> Neighbour;
    public Vertex Parent;
    public bool IsVisited = false;
    public Vertex(GameObject VertexData)
    {
        Data = VertexData;
        Neighbour = new List<Vertex>();
    }
}
#endregion

#region 状态机
enum GameStatus
{
    GameStart,
    SetStart,
    SetFinish,
    StartCalculation,
    CalculateInProgress,
    CalculationCompelete,
    ShowPath,
    Finish
}
#endregion

public class NavigationInGraph : MonoBehaviour {
    Vertex VStart = null;
    Vertex VFinish = null;
    Graph MapGraph;
    Queue<Vertex> Rally = new Queue<Vertex>();
    public GameObject OneWayPath;
    public GameObject NormalPath;
    public GameObject Flag;
    public static List<Vertex> NavPointGroup = new List<Vertex>();
    GameStatus gamestate = GameStatus.GameStart;

    #region 读地图画地图
    void DrawLines(Vertex a, Vertex b)
    {
        if (!a.Data.GetComponent<Transform>() || !b.Data.GetComponent<Transform>())
        {
            return;
        }
        var apos = a.Data.transform.position;
        var bpos = b.Data.transform.position;
        var Center = apos + ((bpos - apos) / 2f);
        GameObject Line = null;
        if (b.Neighbour.Contains(a))
        {
            Line = Instantiate(NormalPath, Center, Quaternion.identity);
        }
        else
        {
            Line = Instantiate(OneWayPath, Center, Quaternion.identity);
        }
        Line.transform.LookAt(bpos);
        var Scale = Line.transform.localScale;
        Scale.z = (bpos - apos).magnitude;
        Line.transform.localScale = Scale;
    }
    void InitGraph()
    {
        MapGraph = new Graph(NavPointGroup.Count);
        MapGraph.Vertixes = NavPointGroup.ToArray();
        MapGraph.SortVertex();
        foreach (Vertex v in MapGraph.Vertixes)
        {
            int[] neighbour = v.Data.GetComponent<NavPoint>().Neighbourhood.ToArray();
            foreach (int id in neighbour)
            {
                v.Neighbour.Add(MapGraph.Vertixes[id]);
            }
        }
        foreach (Vertex v in MapGraph.Vertixes)
        {
            foreach (Vertex link in v.Neighbour)
            {
                MapGraph.Link(v, link);
                DrawLines(v, link);
            }
        }
    }
    #endregion

    #region 适用于图结构的BFS
    bool SetStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            VStart = ClickToGetPoint();
        }
        if (VStart != null)
        {
            return true;
        }
        return false;
    }
    bool SetFinish()
    {
        if (Input.GetMouseButtonDown(0))
        {
            VFinish = ClickToGetPoint();
        }
        if (VFinish != null)
        {
            return true;
        }
        return false;
    }
    Vertex ClickToGetPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 10f);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 10000f))
        {
            if (Hit.collider.GetComponent<NavPoint>())
            {
                Debug.Log("NavPointClick");
                var flag = Instantiate(Flag, Hit.collider.transform.position, Quaternion.identity, Hit.transform);
                flag.transform.position += new Vector3(0, 0.1f, 0);
                return Hit.collider.gameObject.GetComponent<NavPoint>().NavVertex;
            }
            return null;
        }
        return null;
    }

    bool FindPath()                  //搜索节点，当本节点的相邻节点有目标时终止搜索
    {
        Vertex Curr = Rally.Dequeue();
        foreach (Vertex V in Curr.Neighbour)
        {
            if (V==VFinish)
            {
                V.Depth = Curr.Depth + MapGraph.LinkMatrix[Curr.ID,V.ID];
                V.Parent = Curr;
                Debug.Log("PathFound");
                return true;
            }
            if (V.Depth > Curr.Depth + MapGraph.LinkMatrix[Curr.ID, V.ID])
            {
                V.Depth = Curr.Depth + MapGraph.LinkMatrix[Curr.ID, V.ID];
                V.Parent = Curr;
                Rally.Enqueue(V);
            }
        }
        return false;
    }

    IEnumerator BFSforGraph()
    {
        VStart.Depth = 0;
        Rally.Enqueue(VStart);
        while (Rally.Count > 0)
        {
            if (FindPath())
            {
                gamestate = GameStatus.CalculationCompelete;
                break;
            }
            foreach (Vertex v in MapGraph.Vertixes)
            {
                if (v.Depth < int.MaxValue)
                {
                    if (!v.DetectedAndShown)
                    {
                        ShowDetectedPoint(v);
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return 0;
    }
    #endregion

    #region 路线显示
    void ShowDetectedPoint(Vertex v)
    {
        if (v.Depth > 0)
        {
            v.DetectedAndShown = true;
            TextMesh textmesh = v.Data.transform.GetChild(0).GetComponent<TextMesh>();
            textmesh.text = v.Depth.ToString();
            textmesh.color = Color.blue;
            Renderer renderer = v.Data.GetComponent<Renderer>();
            renderer.material.color = Color.green;
        }
    }
    void ShowPath()
    {
        Vertex Child = VFinish;
        while (Child.Parent != null)
        {
            var flag = Instantiate(Flag, Child.Parent.Data.transform.position, Quaternion.identity, Child.Parent.Data.transform);
            flag.transform.position += new Vector3(0, 0.1f, 0);
            Child = Child.Parent;
        }
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        InitGraph();
        gamestate = GameStatus.SetStart;
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (gamestate)
        {
            case GameStatus.SetStart:
                if (SetStart())
                {
                    Debug.Log("StartSet");
                    gamestate = GameStatus.SetFinish;
                }
                break;
            case GameStatus.SetFinish:
                if (SetFinish())
                {
                    if (VFinish == VStart)
                    {
                        Debug.Log("FinishAtStart");
                        VFinish = null;
                        gamestate = GameStatus.SetFinish;
                        break;
                    }
                    Debug.Log("FinishSet");
                    gamestate = GameStatus.StartCalculation;
                }
                break;
            case GameStatus.StartCalculation:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("SearchStart");
                        StartCoroutine(BFSforGraph());
                        gamestate = GameStatus.CalculateInProgress;
                    }
                    break;
                }
            case GameStatus.CalculationCompelete:
                {
                    gamestate = GameStatus.ShowPath;
                    break;
                }
            case GameStatus.ShowPath:
                {
                    gamestate = GameStatus.Finish;
                    ShowPath(); 
                    break;
                }
        }
	}




}
