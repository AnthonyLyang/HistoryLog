using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class NavPoint : MonoBehaviour {
    public int NavID;
    public List<int> Neighbourhood = new List<int>();
    public Vertex NavVertex;
    TextMesh textmesh;

    // Use this for initialization
    void Awake()
    {
        NavVertex = new Vertex(gameObject);
        NavVertex.ID = NavID; 
        NavigationInGraph.NavPointGroup.Add(NavVertex);
    }

    [ContextMenu("PrintID")]
    void PrintID()
    {
        textmesh = transform.GetChild(0).GetComponent<TextMesh>();
        textmesh.text = NavID.ToString();
    }
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
