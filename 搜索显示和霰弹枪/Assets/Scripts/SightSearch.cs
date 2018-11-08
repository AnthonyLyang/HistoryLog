using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSearch : MonoBehaviour {
    public SphereCollider VisionSensor;
    public float VisionRange;
    public float FOV;
    SensingActiveBehavior SensingActive;
    Transform Parent;
	// Use this for initialization
	void Start ()
    {
        VisionSensor.radius = VisionRange;
        Parent = transform.parent;
        SensingActive = Parent.gameObject.GetComponent<SensingActiveBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
        {
            return;
        }
        Debug.Log("TargetInRange");
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
        {
            return;
        }
        if (!IsInFOV(other))
        {
            return;
        }
        Debug.Log("TargetInTrackingRange");
        if (!other.CompareTag("Player"))
        {
            return;
        }
        if (!NoBarrickBetween(other))
        {
            Debug.Log("BarrickBetween");
            return;
        }
        SensingActive.Target = other.gameObject;
        SensingActive.LookAtTarget();
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
        {
            return;
        }
        Debug.Log("TargetOffRange");
        if (other.CompareTag("Player"))
        {
            SensingActive.Target = null;
        }
    }
    bool IsInFOV(Collider other)
    {
        var DIR = (other.transform.position - transform.position).normalized;
        if (Vector3.Dot(DIR,transform.forward)>=FOV)
        {
            //Debug.Log(Mathf.Acos(Vector3.Dot(DIR, transform.forward)));
            return true;
        }
        return false;
    }
    bool NoBarrickBetween(Collider other)
    {
        var Tar = other.gameObject;
        RaycastHit Hit;
        if (Physics.Raycast(Parent.position, Tar.transform.position - Parent.position, out Hit))
        {
            Debug.DrawRay(Parent.position, (Tar.transform.position-Parent.position).normalized*10f, Color.green, 0.3f);
            Debug.Log(Hit);
            if (!Hit.collider.CompareTag(other.tag))
            {
                //if (Hit.collider.transform.parent.gameObject.CompareTag(other.tag))
                //{
                //    return true;
                //}
                Debug.Log(Hit);
                Debug.Log("False");
                return false;
            }
            return true;
        }
        return true;
    }
}
