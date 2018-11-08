using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    HeroCharacter playerCharacter;
    public float TurnSpeed;
	// Use this for initialization
	void Start ()
    {
        playerCharacter = GetComponent<HeroCharacter>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        var h = Input.GetAxis("Horizontal");
        playerCharacter.Move(h);
        if (h != 0)
        {
            var dir = Vector3.right * h;
            playerCharacter.Rotate(dir, TurnSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("konge");
            playerCharacter.Jump();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("g");
            playerCharacter.GrabCheck();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            print("h");
            playerCharacter.Hang();
        }
    }
}
