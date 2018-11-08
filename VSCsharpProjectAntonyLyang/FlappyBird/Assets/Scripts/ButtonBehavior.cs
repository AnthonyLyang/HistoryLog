using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour {
    public GameObject button1;
    public GameObject button2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void On_Click_Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void On_Click_Pause()
    {
        button1.SetActive(false);
        button2.SetActive(true);
        Time.timeScale = 0;
    }
    public void On_Click_Play()
    {
        button1.SetActive(true);
        button2.SetActive(false);
        Time.timeScale = 1;
    }
}
