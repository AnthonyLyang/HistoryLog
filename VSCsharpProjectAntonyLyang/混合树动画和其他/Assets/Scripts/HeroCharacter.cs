using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : MonoBehaviour {
    CharacterController cc;
    Vector3 pendingvelocity;
    public Transform GrabSocket;
    public ParticleSystem Smoke;
    public Transform GrabObject=null;
    public Animator animator;
    public float RunSpeed;
    public float JumpPower;
    bool rotateComplete = true;
    // Use this for initialization
    void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        pendingvelocity.z = 0f;
        if (cc.enabled)
        {
            cc.Move(pendingvelocity * Time.deltaTime);
            pendingvelocity.y += cc.isGrounded ? 0f : Physics.gravity.y * 10f * Time.deltaTime;
            animator.SetFloat("Speed", Mathf.Abs(cc.velocity.x));
            animator.SetFloat("FallSpeed", cc.velocity.y);
            animator.SetBool("IsGrounded", cc.isGrounded);
        }
        AttackCheck();
    }
    public void Move(float inputX)
    {
        pendingvelocity.x = inputX * RunSpeed;
    }
    public void Jump()
    {
        if (cc.isGrounded)
        {
            pendingvelocity.y = JumpPower;
        }
    }
    public void Rotate(Vector3 LookDir, float TurnSpeed)
    {
        rotateComplete = false;
        var targetPos = transform.position + LookDir;
        var characterPos = transform.position;

        //去除Y轴影响
        characterPos.y = 0;
        targetPos.y = 0;
        //角色面朝目标的向量
        Vector3 faceToDir = targetPos - characterPos;
        //角色面朝目标方向的四元数
        if (faceToDir == Vector3.zero)
            return;
        Quaternion faceToQuat = Quaternion.LookRotation(faceToDir);
        //球面插值
        Quaternion slerp = Quaternion.Slerp(transform.rotation, faceToQuat, TurnSpeed * Time.deltaTime);

        if (slerp == faceToQuat)
        {
            rotateComplete = true;
        }
        transform.rotation = slerp;
    }
    public void GrabCheck()
    {
        if (!rotateComplete)
        {
            return;
        }
        else
        {
            if (GrabObject != null && rotateComplete)
            {
                GrabObject.transform.SetParent(null);
                if (GrabObject.GetComponent<Rigidbody>())
                {
                    GrabObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                if(GrabObject.GetComponent<CharacterController>())
                {
                    GrabObject.GetComponent<CharacterController>().enabled = true;
                }
                GrabObject = null;
                animator.SetBool("Grab", false);
            }
            else
            {
                var RayCastPoint = transform.position + Vector3.down * 0.25f;
                RaycastHit Hit;
                Debug.DrawLine(RayCastPoint, RayCastPoint + Vector3.right * (1.5f), Color.green, 10f);
                if (Physics.Raycast(RayCastPoint, transform.forward, out Hit, 1.5f))
                {
                    Debug.Log(Hit.transform.name);
                    Debug.DrawRay(Hit.point, Vector3.up, Color.red, 10f);
                    Debug.DrawLine(Hit.point, RayCastPoint, Color.blue, 10f);
                    if (Hit.collider.CompareTag("Grabable"))
                    {
                        animator.SetBool("Grab", true);
                        GrabObject = Hit.transform;
                        GrabObject.SetParent(GrabSocket);
                        GrabObject.localPosition = Vector3.zero;
                        if (!GrabObject.GetComponent<Rigidbody>())
                        {
                            GrabObject.GetComponent<CharacterController>().enabled = false;
                        }
                        else
                        {
                            GrabObject.localRotation = Quaternion.identity;
                            GrabObject.GetComponent<Rigidbody>().isKinematic = true;
                        }

                    }
                }
            }
        }
    }
    public void Hang()
    {
        if (GrabObject == null || !rotateComplete)
        {
            return;
        }
        else
        {
            GrabObject.transform.SetParent(null);
            GrabObject = null;
            animator.SetBool("Grab", false);
        }
    }






    public void AttackCheck()
    {
        var Dist = cc.height / 2;
        RaycastHit Hit;
        if (Physics.Raycast(transform.position, Vector3.down, out Hit, Dist + 0.5f))
        {
            //Debug.Log(Hit.transform.name);
            //Debug.DrawRay(Hit.point, Vector3.up, Color.black, 1f);
            if (Hit.transform.GetComponent<HeroCharacter>() && Hit.transform != transform)
            {
                Hit.transform.GetComponent<HeroCharacter>().Die();
            }
        }
    }
    public void Die()
    {
        var FX = Instantiate(Smoke, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(FX, 2);
        if (gameObject.GetComponent<HeroCharacter>().GrabObject != null)
        {
            gameObject.GetComponent<HeroCharacter>().GrabObject.transform.SetParent(null);
            if (gameObject.GetComponent<HeroCharacter>().GrabObject.GetComponent<Rigidbody>())
            {
                gameObject.GetComponent<HeroCharacter>().GrabObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        Destroy(gameObject);
    }
}
