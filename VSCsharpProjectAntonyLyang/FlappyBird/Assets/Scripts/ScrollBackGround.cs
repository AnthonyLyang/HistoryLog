using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackGround : MonoBehaviour {
    Rigidbody2D rigid2d;
	// Use this for initialization
	void Awake ()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        rigid2d.velocity = new Vector2(-GameMode.gamemode.GameSpeed, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        rigid2d.velocity = new Vector2(-GameMode.gamemode.GameSpeed, 0);
    }
}
