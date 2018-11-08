using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestMove : MonoBehaviour {
    Vector2 Pos;
    Vector2 Dir;
    float x;
    float y;
    public float Speed;
	// Use this for initialization
	void Start ()
    {

        Pos = GetComponent<RectTransform>().anchoredPosition;
        Dir = Pos.normalized;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(Dir);
        Pos -= Dir * Speed;
        //Debug.Log(Pos);
        GetComponent<RectTransform>().anchoredPosition = Pos;
    
	}

}
