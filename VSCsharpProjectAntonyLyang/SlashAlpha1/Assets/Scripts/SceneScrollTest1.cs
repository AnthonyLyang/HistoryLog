using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScrollTest1 : MonoBehaviour {
    public Transform CharacterTrans;
    public GameObject OtherPartOfScene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void LateUpdate()
    {
        Scroll();
    }
    void Scroll()
    {
        if (CharacterTrans.position.z - transform.position.z >= 50)
        {
            var pos = OtherPartOfScene.transform.position;
            pos += new Vector3(0, 0, 50f);
            transform.position = pos;
        }
    }
}
