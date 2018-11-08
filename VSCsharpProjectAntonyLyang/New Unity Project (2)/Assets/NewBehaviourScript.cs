using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    Rigidbody rigid;
    public float Speed;
	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        rigid.AddTorque(new Vector3(0, 0, Speed) * Time.deltaTime);
        //transform.Rotate(new Vector3(0, 1, 0), Speed);
    }
}
