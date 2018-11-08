using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBeingSensed : MonoBehaviour {
    public SphereCollider SoundSphere;
    public SensingActiveBehavior SensingActive;
    
    public float SoundRange;
    AudioSource Audio;
	// Use this for initialization
	void Start ()
    {
        SoundSphere.radius = SoundRange;
        Audio = GetComponent<AudioSource>();
	}
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
        {
            return;
        }
        Debug.Log("ListenerEntered");
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("AI"))
        {
            return;
        }
        if (!Audio.isPlaying)
        {
            return;
        }
        Debug.Log("BeingHeard");
        if(!other.gameObject.GetComponent<SensingActiveBehavior>())
        {
            return;
        }
        SensingActive = other.gameObject.GetComponent<SensingActiveBehavior>();
        if (SensingActive.Target != null)
        {
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
        Debug.Log("ListenerOut");
        SensingActive.Target = null;
    }
}
