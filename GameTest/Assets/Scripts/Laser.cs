using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    Ray ray;
    LineRenderer lineRenderer;
	// Use this for initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //lineRenderer.SetPosition(0, transform.position);
        ray = new Ray(transform.position, transform.up * (-1f));
        Debug.DrawRay(transform.position, transform.up * (-1f), Color.black, 1f);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 100f))
        {
            if (Hit.collider != null)
            {
                var Endpoint = transform.InverseTransformPoint(Hit.point);
                lineRenderer.SetPosition(1, Endpoint);
            }

        }
	}
}
