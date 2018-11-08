using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour {
    public enum WeaponStatus
    {
        Down,
        ReadyToFire,
        Interval,
        OutofAmmo,
        Loading
    }

    //enum WeaponType
    //{



    //}





    public WeaponStatus weaponstate;
    [Range(1,1000)]
    [SerializeField]
    public int RPM;

#region 武器状态机相关参数
    float FireInterval;
    float IntervalCounter;
    public int AmmoCapacity;
    int CurrentAmmo;
    public float ReloadTime;
    #endregion

    #region 射弹相关参数
    Vector3 FireDir;
    Vector3 Recoil;
    Transform Muzzle;
    public GameObject TracerParticle;
    public GameObject FlareParticle;
    public float WeaponSlideX;
    public float WeaponSlideY;    
    public float RecoilReturnSpeed;
    public float FireRange;
    #endregion

    Animator animator;





    Character character;
	// Use this for initialization
	void Start ()
    {
        animator = GetComponentInParent<Animator>();
        Muzzle = GetComponent<Transform>();
        FireInterval = 60f / RPM;
        character = GetComponentInParent<Character>();
        weaponstate = WeaponStatus.Down;
        CurrentAmmo = AmmoCapacity;
        IntervalCounter = 0;
        FireDir = Muzzle.forward;
        Recoil = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Recoil = Vector3.Lerp(Recoil, Vector3.zero, RecoilReturnSpeed * Time.deltaTime);
        FireDir = Muzzle.forward;
        switch (weaponstate)
        {
            case WeaponStatus.Down:
                break;
            case WeaponStatus.ReadyToFire:
                {
                    //Fire();
                    if (IntervalCounter > 0)
                    {
                        weaponstate = WeaponStatus.Interval;
                    }
                    if (CurrentAmmo <= 0)
                    {
                        weaponstate = WeaponStatus.OutofAmmo;
                    }
                    break;
                }
            case WeaponStatus.Interval:
                {
                    FireCountDown();
                    break;
                }
            case WeaponStatus.OutofAmmo:
                {
                    Reload();
                    break;
                }
            case WeaponStatus.Loading:
                break;
        }
	}

    public void Fire()
    {
        if (weaponstate != WeaponStatus.ReadyToFire)
        {
            return;
        }
        Debug.Log("Fired");
        RoundShot();
        CurrentAmmo -= 1;
        if (CurrentAmmo > 0)
        {
            IntervalCounter = FireInterval;
        }
    }
    void FireCountDown()
    {
        if (IntervalCounter > 0)
        {
            IntervalCounter -= Time.deltaTime;
        }
        if (IntervalCounter <= 0)
        {
            weaponstate = WeaponStatus.ReadyToFire;
        }
    }

    void RoundShot()
    {
        RaycastHit Hit;
        Ray ray = new Ray(Muzzle.position, FireDir * FireRange + Recoil);
        Debug.DrawRay(Muzzle.position, FireDir * FireRange + Recoil, Color.red, 1f);
        var Flare = Instantiate(FlareParticle, Muzzle.position, Quaternion.LookRotation(ray.direction), Muzzle);
        var Tracer = Instantiate(TracerParticle, Muzzle.position, Quaternion.LookRotation(ray.direction), Muzzle);
        Destroy(Tracer, 0.5f);
        Destroy(Flare, 0.3f);
        Recoil += new Vector3(Random.Range(((-1) * WeaponSlideX), WeaponSlideX), Random.Range(0, WeaponSlideY), 0);
        if (Physics.Raycast(ray, out Hit, FireRange))
        {
            if (Hit.collider.CompareTag("Character"))
            {
                return;
            }
        }
        return;
    }














    public void Reload()
    {
        animator.SetTrigger("Reload");
        weaponstate = WeaponStatus.Loading;
        Debug.Log("Reloading");
        StartCoroutine(ReloadCountDown());
    }
    IEnumerator ReloadCountDown()
    {
        yield return new WaitForSeconds(ReloadTime);
        Debug.Log("ReloadCompelete");
        CurrentAmmo = AmmoCapacity;
        weaponstate = WeaponStatus.ReadyToFire;
    }
}
