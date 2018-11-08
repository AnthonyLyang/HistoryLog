using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    public float distance;
    public float height;
    public float lead;
    public float smooth;
    // Use this for initialization
    void Start ()
    {
        transform.position = target.position + target.up * height - target.forward * distance + target.right * lead;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void LateUpdate()
    {
        var pos = target.position + target.up * height - target.forward * distance + target.right * lead;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * smooth);
        //transform.LookAt(target.position);
    }
}
