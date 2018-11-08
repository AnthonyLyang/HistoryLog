using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    float StageFlag = 0f;
    public float AIActionTimerSet;
    public float AIGrabDuration;
    public float AIGrabCooldown;
    public float CannotGrabTimer = 0f;
    public float GrabTimer = 0f;
    CharacterController cc;
    HeroCharacter aicharacter;
    int h;
    // Use this for initialization
    void Start ()
    {
        h = Random.Range(-1, 2);
        cc = GetComponent<CharacterController>();
        aicharacter = GetComponent<HeroCharacter>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (GrabableCheck() && aicharacter.GrabObject == null)
        {
            if (CannotGrabTimer <= 0f)
            {
                aicharacter.GrabCheck();
                if (aicharacter.GrabObject!=null)
                {
                    CannotGrabTimer = AIGrabCooldown;
                }
                
            }
            else
            {
                CannotGrabTimer -= Time.deltaTime;
            }
            
        }
        if (aicharacter.GrabObject != null&&(aicharacter.GrabObject.GetComponent<Rigidbody>()))
        {
            GrabTimer += Time.deltaTime;
            if (GrabTimer >= AIGrabDuration)
            {
                aicharacter.GrabCheck();
                GrabTimer = 0f;
            }
        }
        StageFlag += Time.deltaTime;
        if (StageFlag < AIActionTimerSet)
        {
            aicharacter.Move(h);
            if (h != 0)
            {
                var dir = Vector3.right * h;
                aicharacter.Rotate(dir, 10f);
            }
        }
        else
        {
            h = Random.Range(-1, 2);
            StageFlag = 0f;
        }
		
	}
    bool GrabableCheck()
    {
        if (cc.enabled)
        {
            var Col = Physics.OverlapSphere(transform.position, (1.5f));
            foreach (Collider col in Col)
            {
                if (col.gameObject.CompareTag("Grabable"))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
