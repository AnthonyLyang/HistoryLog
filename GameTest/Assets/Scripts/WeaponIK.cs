using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIK : MonoBehaviour {

    public Transform LefthandIK;
    public Animator animator;
    public GunFire gunfire;
    bool IKActive = false;
    bool IKtriggered = false;
    Character character;

    // Use this for initialization
    private void Awake()
    {
        gunfire = GetComponentInChildren<GunFire>();
        character = GetComponentInParent<Character>();
    }

    void Start ()
    {

	}
	// Update is called once per frame
	void Update ()
    {
        if (character.IsAiming)
        {
            if (!IKtriggered)
            {
                if (gunfire.gameObject.activeSelf)
                {
                    StartCoroutine(LeftHandIKSet());
                    IKtriggered = true;
                }
            }
        }
        else
        {
            IKActive = false;
            IKtriggered = false;
        }
	}
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            if (IKActive)
            {
                Debug.Log("IK");
                var weight = animator.GetIKPositionWeight(AvatarIKGoal.LeftHand);
                //Debug.Log(weight);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, LefthandIK.position);
                //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

                //animator.SetIKRotation(AvatarIKGoal.LeftHand, LefthandIK.rotation);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
            }
        }
    }
    IEnumerator LeftHandIKSet()
    {
        yield return new WaitForSeconds(0.42f);
        IKActive = true;
        if (gunfire.weaponstate == GunFire.WeaponStatus.Down)
        {
            gunfire.weaponstate = GunFire.WeaponStatus.ReadyToFire;
        }
        else if (gunfire.weaponstate == GunFire.WeaponStatus.Loading)
        {
            gunfire.weaponstate = GunFire.WeaponStatus.OutofAmmo;
        }
    }
}
