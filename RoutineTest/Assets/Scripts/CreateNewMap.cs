using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Pos
{
    public int x = 0;
    public int y = 0;
    public Pos()
    {

    }
    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public bool Equals(Pos p)
    {
        return x == p.x && y == p.y;
    }
    public static int AStarDistance(Pos p1, Pos p2)
    {
        int d1 = Mathf.Abs(p1.x - p2.x);
        int d2 = Mathf.Abs(p1.y - p2.y);
        return d1 + d2;
    }
}

public class AStarScore
{
    public int G=0;
    public int H=0;
    public int F { get { return (G + H); } }
    public bool Closed = false;
    public Pos parent = null;
    public int FCompareTo(AStarScore a)
    {
        if (F == a.F)
        {
            return 0;
        }
        else if (F>a.F)
        {
            return 1;
        }
        return -1;
    }
    public AStarScore(int g, int h)
    {
        G = g;
        H = h;
        Closed = false;
    }
}

public class CreateNewMap : MonoBehaviour
{
    public const int START = 8;
    public const int END = 9;
    public const int WALL = 1;
    int W = 30;
    int H = 30;
    int[,] MapGrid;
    public GameObject wall;
    public GameObject player;
    public GameObject Flood;

    Pos startPos;
    Pos endPos;
    GameObject pathParent;
    delegate bool Func(Pos cur, int ox, int oy);
    int cur_depth = 0;


    AStarScore[,] AStarSearch;
    enum GameState
    {
        SetBeginPoint,
        SetEndPoint,
        StartCalculation,
        Calculation,
        ShowPath,
        Finish,
    }
    GameState gameState = GameState.SetBeginPoint;


    #region 读地图加载地图
    void ReadMap()
    {
        string path = Application.dataPath + "/Resource/" + "map2.txt";
        if (!File.Exists(path))
        {
            Debug.Log("File does not exist");
            return;
        }
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);
        string strReadLine = "";
        int y = 0;
        read.ReadLine();
        strReadLine = read.ReadLine();
        while (strReadLine != null && y < H)
        {
            for (int x = 0; x < W && x < strReadLine.Length; ++x)
            {
                int t;
                switch (strReadLine[x])
                {
                    case '1':
                        t = 1;
                        break;
                    case '8':
                        t = 8;
                        break;
                    case '9':
                        t = 9;
                        break;
                    default:
                        t = 0;
                        break;
                }
                MapGrid[y, x] = t;
            }
            y += 1;
            strReadLine = read.ReadLine();
        }
        read.Dispose();
        fs.Close();
    }
    void InitMap()
    {
        var walls = new GameObject();
        walls.name = "walls";
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (MapGrid[i, j] == WALL)
                {
                    var go = Instantiate(wall, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity);
                    go.transform.parent = walls.transform;
                }
            }
        }
    }
    #endregion

    #region Breadth First Search
    short[,] Search_Result= null;
    IEnumerator BFS()
    {
        Search_Result = new short[MapGrid.GetLength(0), MapGrid.GetLength(1)];
        for (int i = 0; i < H; i++)//初始化 最大值=未搜索
        {
            for (int j = 0; j < W; j++)
            {
                Search_Result[i, j] = short.MaxValue;
            }
        }
        Queue<Pos> Rally = new Queue<Pos>();
        Func func = (Pos cur, int ox, int oy) =>
          {
              if (MapGrid[cur.y + oy, cur.x + ox] == END)
              {
                  Search_Result[cur.y + oy, cur.x + ox] = (short)(Search_Result[cur.y, cur.x] + 1);
                  Debug.Log("PathFound");
                  return true;
              }
              if (MapGrid[cur.y + oy, cur.x + ox] == 0)
              {
                  if (Search_Result[cur.y + oy, cur.x + ox] > Search_Result[cur.y, cur.x] + 1)
                  {
                      Search_Result[cur.y + oy, cur.x + ox] = (short)(Search_Result[cur.y, cur.x] + 1);
                      Rally.Enqueue(new Pos(cur.x + ox, cur.y + oy));
                  }
              }
              return false;
          };
        Search_Result[startPos.y, startPos.x] = 0;
        Rally.Enqueue(startPos);
        while (Rally.Count > 0)//上下左右逐格搜索路线搜到空路线就加队列里
        {
            Pos cur = Rally.Dequeue();
            if (cur.y > 0)
            {
                if (func(cur, 0, -1))
                {
                    gameState = GameState.ShowPath;
                    break;
                }    
            }
            if (cur.y < H - 1)
            {
                if (func(cur, 0, 1))
                {
                    gameState = GameState.ShowPath;
                    break;
                }
            }
            if (cur.x > 0)
            {
                if (func(cur, -1, 0))
                {
                    gameState = GameState.ShowPath;
                    break;
                }
            }
            if (cur.x < W-1)
            {
                if (func(cur, 1, 0))
                {
                    gameState = GameState.ShowPath;
                    break;
                }
            }
            if (Search_Result[cur.y, cur.x] > cur_depth)
            {
                cur_depth = Search_Result[cur.y, cur.x];
                RefreshFlood(Search_Result);
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
    #endregion

    #region DFS
    //IEnumerator DFS()
    //{
    //    Search_Result = new short[MapGrid.GetLength(0), MapGrid.GetLength(1)];
    //    for (int i = 0; i < H; i++)//初始化 最大值=未搜索
    //    {
    //        for (int j = 0; j < W; j++)
    //        {
    //            Search_Result[i, j] = short.MaxValue;
    //        }
    //    }
    //    Queue<Pos> Rally = new Queue<Pos>();
    //    Func func = (Pos cur, int ox, int oy) =>
    //    {
    //        if (MapGrid[cur.y + oy, cur.x + ox] == END)
    //        {
    //            Search_Result[cur.y + oy, cur.x + ox] = (short)(Search_Result[cur.y, cur.x] + 1);
    //            Debug.Log("PathFound");
    //            return true;
    //        }
    //        if (MapGrid[cur.y + oy, cur.x + ox] == 0)
    //        {
    //            if (Search_Result[cur.y + oy, cur.x + ox] > Search_Result[cur.y, cur.x] + 1)
    //            {
    //                Search_Result[cur.y + oy, cur.x + ox] = (short)(Search_Result[cur.y, cur.x] + 1);
    //                Rally.Enqueue(new Pos(cur.x + ox, cur.y + oy));
    //            }
    //        }
    //        return false;
    //    };
    //    Search_Result[startPos.y, startPos.x] = 0;
    //    Rally.Enqueue(startPos);
    //    while (Rally.Count > 0)
    //    {
    //        Pos cur = Rally.Dequeue();
    //        if (cur.y > 0)
    //        {
    //            if (func(cur, 0, -1))
    //            {
    //                gameState = GameState.ShowPath;
    //                break;
    //            }
    //            if (Rally.Count == 0)
    //            {
    //                Rally.Enqueue(cur);
    //                cur = Rally.Dequeue();
    //                if (cur.x > 0)
    //                {
    //                    if (func(cur, -1, 0))
    //                    {
    //                        gameState = GameState.ShowPath;
    //                        break;
    //                    }
    //                    if (Rally.Count == 0)
    //                    {
    //                        Rally.Enqueue(cur);
    //                        cur = Rally.Dequeue();
    //                        if (cur.y < H - 1)
    //                        {
    //                            if (func(cur, 0, 1))
    //                            {
    //                                gameState = GameState.ShowPath;
    //                                break;
    //                            }
    //                            if (Rally.Count == 0)
    //                            {
    //                                Rally.Enqueue(cur);
    //                                cur = Rally.Dequeue();
    //                                if (cur.x < W - 1)
    //                                {
    //                                    if (func(cur, 1, 0))
    //                                    {
    //                                        gameState = GameState.ShowPath;
    //                                        break;
    //                                    }
    //                                }
    //                                if (Rally.Count == 0)
    //                                {
    //                                    Rally.Enqueue(cur);
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        if (Search_Result[cur.y, cur.x] > cur_depth)
    //        {
    //            cur_depth = Search_Result[cur.y, cur.x];
    //            RefreshFlood(Search_Result);
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //    }
    //    yield return null;
    //}





    // Use this for initialization
    #endregion

    #region AStar
    IEnumerator AStar()
    {
        AStarSearch = new AStarScore[MapGrid.GetLength(0), MapGrid.GetLength(1)];
        List<Pos> List = new List<Pos>();
        AStarSearch[startPos.y, startPos.x] = new AStarScore(0, 0);
        List.Add(startPos);
        Func func = (Pos cur, int ox, int oy) => 
        {
            var o_score = AStarSearch[cur.y + oy, cur.x + ox];
            if (o_score != null && o_score.Closed)
            {
                return false;
            }
            var cur_score = AStarSearch[cur.y, cur.x];
            Pos o_pos = new Pos(cur.x + ox, cur.y + oy);
            if (MapGrid[cur.y + oy, cur.x + ox] == END)
            {
                var a = new AStarScore(cur_score.G + 1, 0);
                AStarSearch[cur.y + oy, cur.x + ox] = a;
                a.parent = cur;
                Debug.Log("PathFound");
                return true;
            }
            if (MapGrid[cur.y + oy, cur.x + ox] == 0)
            {
                if (o_score == null)
                {
                    var a = new AStarScore(cur_score.G + 1, Pos.AStarDistance(o_pos, endPos));
                    AStarSearch[cur.y + oy, cur.x + ox] = a;
                    List.Add(o_pos);
                    a.parent = cur;
                }
                else if (o_score.G > cur_score.G + 1)
                {
                    o_score.G = cur_score.G + 1;
                    o_score.parent = cur;
                    if (!List.Contains(o_pos))
                    {
                        List.Add(o_pos);
                    }
                }
            }
            return false;
        };
        while (List.Count > 0)
        {
            List.Sort((Pos p1, Pos p2) =>
            {
                AStarScore a1 = AStarSearch[p1.y, p1.x];
                AStarScore a2 = AStarSearch[p2.y, p2.x];
                return a1.FCompareTo(a2);
            });
            Pos cur = List[0];
            List.RemoveAt(0);
            AStarSearch[cur.y, cur.x].Closed = true;
            if (cur.y > 0)
            {
                if (func(cur, 0, -1))
                {
                    break;
                }
            }
            if (cur.y < H - 1)
            {
                if (func(cur, 0, 1))
                {
                    break;
                }
            }
            if (cur.x > 0)
            {
                if (func(cur, -1, 0))
                {
                    break;
                }
            }
            if (cur.x < W - 1)
            {
                if (func(cur, 1, 0))
                {
                    break;
                }
            }
            short[,] Draw = new short[MapGrid.GetLength(0), MapGrid.GetLength(1)];
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    Draw[i, j] = short.MaxValue;
                    if (AStarSearch[i, j] != null)
                    {
                        Draw[i, j] = (short)AStarSearch[i, j].F;
                    }
                }
            }
            RefreshFlood(Draw);
            yield return 0;/*new WaitForSeconds(0.1f);*/
        }
        gameState = GameState.ShowPath;
        yield return null;
    }



    #endregion
    void Start ()
    {
        pathParent = GameObject.Find("PathParent");
        MapGrid = new int[H, W];
        ReadMap();
        InitMap();
	}
    bool SetPoint(int n)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // We need to actually hit an object
            RaycastHit hitt = new RaycastHit();
            Physics.Raycast(ray, out hitt, 10000);
            if (hitt.transform != null && hitt.collider.CompareTag("Ground"))
            {
                int x = (int)hitt.point.x;
                int y = (int)hitt.point.z;
                if (x < 0 || y < 0)
                {
                    return false;
                }
                MapGrid[y, x] = n;
                if (n == START)
                {
                    startPos = new Pos(x, y);
                }
                else if (n == END)
                {
                    endPos = new Pos(x, y);
                }
                return true;
            }
        }
        return false;
    }

#region 按计算得的路线刷新显示
    void Refresh()
    {
        GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
        foreach (var go in all_go)
        {
            
            Destroy(go);
        }
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (MapGrid[i, j] == START)
                {
                    var go = Instantiate(player, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
                if (MapGrid[i, j] == END)
                {
                    var go = Instantiate(player, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
            }
        }

    }
    #endregion
    #region 探到哪刷到哪
    void RefreshFlood(short[,] temp)
    {
        Refresh();
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (MapGrid[i, j] == 0 && temp[i, j] != short.MaxValue)
                {
                    var go = Instantiate(Flood, new Vector3(j * 1, 0.1f, i * 1), Quaternion.identity, pathParent.transform);
                    go.tag = "Path";
                }
            }
        }
    }
    #endregion
    void ShowPath()
    {
        //Refresh();
        Pos p = new Pos(endPos.x, endPos.y);
        short step;
        while (!p.Equals(startPos))
        {
            step = Search_Result[p.y, p.x];
            if (p.y > 0 && Search_Result[p.y - 1, p.x] == step - 1)
            {
                p = new Pos(p.x, p.y - 1);
                var go = Instantiate(player, new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, pathParent.transform);
                go.tag = "Path";
            }
            else if (p.y < H - 1 && Search_Result[p.y + 1, p.x] == step - 1)
            {
                p = new Pos(p.x, p.y + 1);
                var go = Instantiate(player, new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, pathParent.transform);
                go.tag = "Path";
            }
            else if (p.x > 0 && Search_Result[p.y, p.x - 1] == step - 1)
            {
                p = new Pos(p.x - 1, p.y);
                var go = Instantiate(player, new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, pathParent.transform);
                go.tag = "Path";
            }
            else if (p.x<W-1 && Search_Result[p.y, p.x + 1] == step - 1)
            {
                p = new Pos(p.x + 1, p.y);
                var go = Instantiate(player, new Vector3(p.x * 1, 0.5f, p.y * 1), Quaternion.identity, pathParent.transform);
                go.tag = "Path";
            }
        }
    }
    void ShowPathAStar()
    {
        Pos pos = endPos;
        while (!pos.Equals(startPos))
        {
            var go = Instantiate(player, new Vector3(pos.x * 1, 0.5f, pos.y * 1), Quaternion.identity, pathParent.transform);
            pos = AStarSearch[pos.y, pos.x].parent;
        }

    }
    // Update is called once per frame
    void Update ()
    {
        switch (gameState)
        {
            case GameState.SetBeginPoint:
                if (SetPoint(START))
                {
                    Debug.Log("StateSwitch");
                    Refresh();
                    gameState = GameState.SetEndPoint;
                }
                break;
            case GameState.SetEndPoint:
                if (SetPoint(END))
                {
                    Debug.Log("StateSwitch");
                    Refresh();
                    gameState = GameState.StartCalculation;
                }
                break;
            case GameState.StartCalculation:
                {
                    Debug.Log("Start");
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("StateSwitch");
                        StartCoroutine(BFS());
                        gameState = GameState.Calculation;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        StartCoroutine(AStar());
                    }

                }
                
                break;
            case GameState.Calculation:
                break;
            case GameState.ShowPath:
                ShowPath();
                //ShowPathAStar();
                gameState = GameState.Finish;
                break;
            case GameState.Finish:
                break;
        }
    }

}
