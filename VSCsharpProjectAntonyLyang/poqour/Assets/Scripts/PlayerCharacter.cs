using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {
    public Rigidbody rigid;
    public Animator animator;
    public float Speed;
    public float JumpForce;
    public float BufferSpeed;
    // Use this for initialization
    private void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var vel = rigid.velocity;
        animator.SetBool("IsStanding", IsStanding(vel));
        animator.SetBool("IsPullingUp",IsPullingUp(vel));
        animator.SetBool("InGround", IsInGround());
	}
    public void Move()
    {
        if(IsInGround()){
            var vel = rigid.velocity;
            //vel = new Vector3((Vector3.right * Speed).x, 0f, 0f);
            //rigid.velocity = vel;
            vel.x = Speed;
            rigid.velocity = vel;
            animator.SetTrigger("Run");
        }
    }
    public void Jump()
    {
        if (IsInGround())
        {
            rigid.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
    public void Die()
    {

    }
    public bool IsStanding(Vector3 vec)
    {
        if (vec.magnitude<=BufferSpeed)
        {
            return true;
        }
        else
            return false;
    }
    public bool IsPullingUp(Vector3 vec)
    {
        if (vec.y > 0)
            return true;
        else
            return false;
    }
    public bool IsInGround()
    {
        Collider[] colliders = Physics.OverlapSphere(rigid.position, 0.135f);
        foreach (Collider c in colliders)
        {
            if (c.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}
