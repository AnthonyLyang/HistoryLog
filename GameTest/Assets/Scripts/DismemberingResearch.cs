using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismemberingResearch : MonoBehaviour {
    public GameObject WholeBody;
    GameObject BodyPartTemp;
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponentInChildren<Animator>();
        Time.timeScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            GetACopyofPresentState();
        }
	}
    void GetACopyofPresentState()
    {
        WholeBody = transform.GetChild(0).gameObject;
        BodyPartTemp = Instantiate(WholeBody, transform);
        anim = BodyPartTemp.GetComponent<Animator>();


    }
}
