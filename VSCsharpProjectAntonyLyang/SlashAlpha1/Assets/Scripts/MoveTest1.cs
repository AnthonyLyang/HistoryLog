using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest1 : MonoBehaviour {
    Rigidbody rigid;
	// Use this for initialization
	void Start ()
    {
        rigid =gameObject.GetComponent<Rigidbody>();
        rigid.velocity = new Vector3(0, 0, 20f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        rigid.velocity = new Vector3(0, 0, 20f);
    }
}
