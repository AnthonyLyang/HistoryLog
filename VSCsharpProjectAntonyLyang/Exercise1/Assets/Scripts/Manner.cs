using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manner : MonoBehaviour {
    public Rigidbody rigid;
    public float moveSpeed;
    public float rotateSpeed;
    public float FireSpeed;
    public Transform Muzzle;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Move(float v)
    {
        rigid.velocity = transform.forward * v * moveSpeed;
    }
    public void Rotate(float v)
    {
        transform.Rotate(Vector3.up * v * rotateSpeed);
    }
    public void Shoot()
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = Muzzle.position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        var r = obj.AddComponent<Rigidbody>();
        r.velocity = transform.forward * FireSpeed;
        Destroy(obj, 2);
    }
}
