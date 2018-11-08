using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingActiveBehavior : MonoBehaviour {
    public GameObject Target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LookAtTarget()
    {
        if (Target == null)
        {
            return;
        }
        var Pos = Target.transform.position;
        Pos.y = transform.position.y;
        transform.LookAt(Pos);
    }
}
