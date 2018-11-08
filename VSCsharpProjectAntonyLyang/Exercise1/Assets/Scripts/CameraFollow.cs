using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float distance;
    public float height;
    public float smooth;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        var pos = target.position + target.up * height - target.forward * distance;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * smooth);
        transform.LookAt(target.position);
    }
}
