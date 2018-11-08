using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public PlayerCharacter playerCharacter;
    // Use this for initialization
    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
    }
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            playerCharacter.Jump();
        }
        if (Input.GetKey(KeyCode.V))
        {
            playerCharacter.Move();
        }
    }
}
