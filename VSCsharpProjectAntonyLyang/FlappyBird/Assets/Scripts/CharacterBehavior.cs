using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {
    public Rigidbody2D rigid2d;
    public Animator animator;
    public float JumpForce;
    [HideInInspector]
    public int Score;
    public bool IsDead=false;

    // Use this for initialization
    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
        if (IsDead)
        {
            GameMode.gamemode.GameOver();
        }
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }
    public void Flap()
    {
        var vel = rigid2d.velocity;
        vel = new Vector2(0, 0);
        rigid2d.velocity = vel;
        rigid2d.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Flap");
    }
    public void Die()
    {
        IsDead = true;
        animator.SetBool("IsDead", true);
    }
}
