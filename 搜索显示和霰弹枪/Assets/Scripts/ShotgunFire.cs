using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunFire : MonoBehaviour {
    public float FirePower;
    public GameObject Shell;
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject shell = ShellEnable();
            AddRandomForce(shell);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    void Test()
    {
        Debug.DrawRay(transform.position, (transform.up*10f + Random.insideUnitSphere), Color.red, 100f);
    }

    void AddRandomForce(GameObject Shell)
    {
        Rigidbody rigid = Shell.GetComponent<Rigidbody>();
        rigid.AddForce((transform.up * 10f + Random.insideUnitSphere) * FirePower, ForceMode.Impulse);
    }

    GameObject ShellEnable()
    {
        GameObject shell = Instantiate(Shell, transform.position, Quaternion.identity);
        shell.SetActive(true);
        return shell;
    }
}
