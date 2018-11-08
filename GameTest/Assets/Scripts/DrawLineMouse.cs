using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum MouseState
{
    Waiting,
    Drawing,
    DrawFinished
}


public class DrawLineMouse : MonoBehaviour {
    MouseState mousestate;
    LineRenderer Line;
    MeshFilter Mesh;


    Vector3 DrawPoint;
    Vector3[] PointGroup;
    Vector3[] DrawMeshTemp;
    Vector3[] MeshGroup;
    int PointCount = 0;
    public int Capability;
    // Use this for initialization
	void Start ()
    {
        Line = GetComponent<LineRenderer>();
        Mesh = GetComponent<MeshFilter>();
        mousestate = MouseState.Waiting;
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (mousestate)
        {


            case MouseState.Waiting:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        PointCount = 0;
                        PointGroup = new Vector3[Capability];
                        DrawMeshTemp = new Vector3[Capability];
                        MeshGroup = new Vector3[4];
                        mousestate = MouseState.Drawing;
                    }
                    break;
                }
            case MouseState.Drawing:
                {
                    if (Input.GetMouseButton(0))
                    {
                        SetPoint();
                        SetLine();
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        mousestate = MouseState.DrawFinished;
                    }
                    break;
                }
            case MouseState.DrawFinished:
                {
                    ClearMesh();
                    if (PointCount > 0)
                    {
                        ClearLine();
                        break;
                    }
                    mousestate = MouseState.Waiting;
                    break;
                }
        }
	}


    void SetPoint()
    {
        DrawPoint = GetMousePos();
        if (PointCount < Capability)
        {
            PointGroup[PointCount] = DrawPoint;
            DrawMeshTemp[PointCount] = DrawPoint;
            DrawMeshTemp[PointCount].z -= 10f;
            PointCount++;
        }
        else
        {
            for (int i = 0; i < Capability - 1; i++)
            {
                PointGroup[i] = PointGroup[i + 1];
                PointGroup[Capability - 1] = DrawPoint;
                DrawMeshTemp[i] = DrawMeshTemp[i + 1];
                DrawMeshTemp[Capability - 1] = DrawPoint;
                DrawMeshTemp[Capability - 1].z -= 10f;
            }
        }
        DrawMesh();
    }

    void SetLine()
    {
        if (PointCount < Capability)
        {
            Line.positionCount = PointCount;
            for (int i = 0; i < PointCount; i++)
            {
                Line.SetPosition(i, PointGroup[i]);
            }
        }
        else
        {
            Line.positionCount = Capability;
        }
        Line.SetPositions(PointGroup);
    }

    void DrawMesh()
    {
        if (PointCount > 2)
        {
            MeshGroup[0] = PointGroup[PointCount - 2];
            MeshGroup[1] = DrawMeshTemp[PointCount - 2];
            MeshGroup[2] = PointGroup[PointCount - 1];
            Mesh.mesh.vertices = MeshGroup;
            Mesh.mesh.triangles = new int[] { 0, 1, 2 };
            if (!gameObject.GetComponent<MeshCollider>())
            {
                gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    void ClearMesh()
    {
        Mesh.mesh = null;
    }








    void ClearLine()
    {
        for (int i = 0; i < PointCount - 1; i++)
        {
            PointGroup[i] = PointGroup[i + 1];
        }
        PointCount -= 1;
        Line.positionCount = PointCount;
        for (int i = 0; i < PointCount; i++)
        {
            Line.SetPosition(i, PointGroup[i]);
        }
    }




    Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 5f);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 10000f, 1<<9))
        {
            var Pos = Hit.point;
            return Pos;
        }
        return Vector3.zero;
    }
}
