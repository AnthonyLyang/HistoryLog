using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidetoWin : MonoBehaviour {
    public GameObject Canvas;
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        if (WinCheck())
        {
            Time.timeScale = 0;
            Canvas.SetActive(true);
            this.enabled = false;
        }
	}
    bool WinCheck()
    {
        var colliders = Physics.OverlapSphere(transform.position, (1.2f));
        foreach (var col in colliders)
        {
            if (col.gameObject.GetComponent<PlayerController>())
            {
                Debug.Log("111");
                colliders = Physics.OverlapSphere(transform.position, (1.2f));
                return true;
            }
        }
        return false;
    }
}
