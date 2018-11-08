using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    public Transform target;
    public float Distance;
    public float Height;
    public float Smooth;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var pos = target.position + target.up * Height + target.right * Distance;
        var offset = target.position - transform.position;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * Smooth);
        transform.rotation = Quaternion.LookRotation(offset);
        //transform.LookAt(target);
	}
}
