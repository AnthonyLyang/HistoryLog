using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundReset : MonoBehaviour
{
    BoxCollider2D GroundCollider;
    float HorizontalLength;
	// Use this for initialization
	void Start ()
    {
        GroundCollider = GetComponent<BoxCollider2D>();
        HorizontalLength = GroundCollider.size.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.x < -HorizontalLength)
        {
            Vector2 offset = new Vector2(HorizontalLength * 2f - 0.1f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
	}
}
