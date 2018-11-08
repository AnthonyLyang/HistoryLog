using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCut : MonoBehaviour {
    public GameObject GlobalController;
    MeshFilter ClipMesh;
    Mesh MeshSelf;
    Vector3 ClipMeshNormal;


    bool[] AboveClipMesh;

    Mesh A = new Mesh();
    Mesh B = new Mesh();

    List<Vector3> AVertex = new List<Vector3>();
    List<Vector3> BVertex = new List<Vector3>();


	// Use this for initialization
	void Start ()
    {
        MeshSelf = GetComponent<MeshFilter>().mesh;
        ClipMesh = GlobalController.GetComponent<MeshFilter>();

        AboveClipMesh = new bool[MeshSelf.vertexCount];

	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    void GetNormalFromClipMesh()
    {
        var Dir1 = ClipMesh.mesh.vertices[1] - ClipMesh.mesh.vertices[0];
        var Dir2 = ClipMesh.mesh.vertices[2] - ClipMesh.mesh.vertices[0];
        ClipMeshNormal = Vector3.Cross(Dir1, Dir2);
    }

    void SplitVertexIntoAboveAndBeneth()
    {
        for (int i = 0; i < AboveClipMesh.Length; i++)
        {
            Vector3 vert = MeshSelf.vertices[i];
            if (Vector3.Dot(vert - transform.position, ClipMeshNormal) >= 0)
            {
                AboveClipMesh[i] = true;
                A.vertices[i] = vert;
            }
            else
            {
                AboveClipMesh[i] = false;
                B.vertices[i] = vert;
            }
        }
    }






}
