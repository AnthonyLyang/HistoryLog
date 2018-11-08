using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    Character character;
    public GameObject WeaponSocket;
    Animator animator;
    GunFire Gun;

    GameObject RangedWeapon;
    GameObject MeleeWeapon;

    // Use this for initialization
    private void Awake()
    {
        character = GetComponent<Character>();
        Gun = GetComponentInChildren<GunFire>();
        animator = GetComponentInChildren<Animator>();
        RangedWeapon = Gun.gameObject.transform.parent.gameObject;
        MeleeWeapon = null;
    }


    void Start ()
    {
        RangedWeapon.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        character.InputMoveV = Input.GetAxis("Vertical");
        character.InputMoveH = Input.GetAxis("Horizontal");
        if (Input.GetMouseButtonDown(1))
        {
            RangedWeapon.SetActive(true);
            character.AimingControllerIn = true;            
        }
        if (Input.GetMouseButtonUp(1))
        {
            character.AimingControllerOut = true;
            Invoke("DropGun", 0.3f);
        }
        character.Aim();
        character.UpdateMove();
        if (Gun.gameObject.activeSelf)
        {
            if (character.IsAiming)
            {
                if (Input.GetMouseButton(0))
                {
                    animator.SetTrigger("Fire");
                    Gun.Fire();
                }
                if (Gun.weaponstate!=GunFire.WeaponStatus.Loading && Input.GetKeyDown(KeyCode.R))
                {
                    Gun.Reload();
                }
            }
        }
    }
    void DropGun()
    {
        RangedWeapon.SetActive(false);
    }



}
