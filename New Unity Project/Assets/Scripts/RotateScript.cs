using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {
    Rigidbody rigid;
    public float RotateSpeed;
	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        //rigid.AddTorque(new Vector3(RotateSpeed, 0, 0));
    }
	
	// Update is called once per frame
	void Update ()
    {
        rigid.AddTorque(new Vector3(RotateSpeed, 0, 0), ForceMode.Impulse);
        //rigid.AddTorque(new Vector3(RotateSpeed, 0, 0));
        //transform.Rotate(new Vector3(1, 0, 0), RotateSpeed * Time.deltaTime, Space.World);
    }
}
