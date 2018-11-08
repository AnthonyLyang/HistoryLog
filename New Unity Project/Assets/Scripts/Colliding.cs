using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour {
    public int CollideCounter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        CollideCounter += 1;
        if (CollideCounter > 1)
        {
            CollideCounter = 0;
            Debug.Log("扔钥匙没碰到柜子上弹回来砸我脑门上了");
        }
        else
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
            Debug.Log("552扔钥匙");
        }

    }
}
