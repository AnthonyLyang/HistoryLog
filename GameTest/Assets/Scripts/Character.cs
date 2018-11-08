using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    #region 移动相关参数
    Rigidbody rigid;
    Animator animator;
    Vector3 velocity;
    public float AngularSpeed;
    public float MoveSpeed;
    float TurnAmount;
    float SlideAmount;
    float MoveAmount;
    public float InputMoveV;
    public float InputMoveH;
    #endregion
    #region Aiming相关参数

    [HideInInspector]

    public bool AimingControllerIn = false;
    public bool AimingControllerOut = false;
    public bool IsAiming = false;
    [HideInInspector]


    GameObject CursorInstance = null;
    public GameObject Cursor;
    Vector3 CursorPosition;

    #endregion



    // Use this for initialization
    void Start ()
    {
        rigid = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
	// Update is called once per frame
	void FixedUpdate ()
    {

	}
    
    
    #region Player
    void Move()
    {
        var MoveVec = InputMoveV * Vector3.forward + InputMoveH * Vector3.right;
        if (MoveVec.magnitude > 1f)
        {
            MoveVec = MoveVec.normalized;
        }
        MoveVec = transform.InverseTransformDirection(MoveVec);
        MoveVec = Vector3.ProjectOnPlane(MoveVec, new Vector3(0,1,0));
        TurnAmount = Mathf.Atan2(MoveVec.x, MoveVec.z);
        MoveAmount = MoveVec.magnitude;
        if (IsAiming)
        {
            var AimingMoveSpeed = MoveSpeed * 0.2f;
            transform.LookAt(CursorInstance.transform);
            MoveAmount = MoveVec.z;
            SlideAmount = MoveVec.x;
            velocity = (transform.forward * MoveAmount + transform.right * SlideAmount) * AimingMoveSpeed;
        }
        else
        {
            transform.Rotate(0, TurnAmount * AngularSpeed * Time.deltaTime, 0);
            velocity = transform.forward * MoveAmount * MoveSpeed;
        }
    }
    public void UpdateMove()
    {
        InputMoveV = Input.GetAxis("Vertical");
        InputMoveH = Input.GetAxis("Horizontal");
        Move();
        animator.SetFloat("MoveSpeed", MoveAmount, 0.01f, Time.deltaTime);
        animator.SetFloat("RotateSpeed", TurnAmount, 0.01f, Time.deltaTime);
        animator.SetBool("IsAiming", IsAiming);
        animator.SetFloat("SlideAmount", SlideAmount, 0.01f, Time.deltaTime);
        rigid.MovePosition(rigid.position + velocity * Time.fixedDeltaTime);
    }

    void UpdateAim()
    {

    }

    public void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 10000f))
        {
            var pos = Hit.point;
            pos.y = transform.position.y;
            CursorPosition = pos;
        }
        if (AimingControllerIn)
        {
            AimingControllerIn = false;
            if (IsAiming)
            {
                return;
            }
            CursorInstance = Instantiate(Cursor, CursorPosition, Quaternion.identity);
            IsAiming = true;
        }
        if (AimingControllerOut)
        {
            AimingControllerOut = false;
            Destroy(CursorInstance);
            IsAiming = false;
        }
        if (CursorInstance != null)
        {
            CursorInstance.transform.position = CursorPosition;
            CursorInstance.transform.LookAt(transform);
        }
    }
    #endregion

}
