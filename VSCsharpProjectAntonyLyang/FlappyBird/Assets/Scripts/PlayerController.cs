using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    CharacterBehavior CharacterBehavior;
    // Use this for initialization
    private void Awake()
    {
        CharacterBehavior = gameObject.GetComponent<CharacterBehavior>();
    }
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterBehavior.Flap();
        }
	}
}
