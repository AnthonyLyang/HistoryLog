using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    Manner manner;
	// Use this for initialization
	void Start ()
    {
        manner = gameObject.GetComponent<Manner>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        manner.Move(v);
        manner.Rotate(h);
        if (Input.GetKeyDown(KeyCode.G))
        {
            manner.Shoot();
        }
	}
}
