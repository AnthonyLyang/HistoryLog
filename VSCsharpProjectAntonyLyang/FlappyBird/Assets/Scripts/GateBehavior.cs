using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehavior : MonoBehaviour {
    GameObject Gate;
    public GameObject Prototype;
    float height;
    public float RespawnPosition;
    public float DestroyTime=10f;
    float Timer;
    float yMin = 0.16f;
    float yMax = 4.93f;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Timer += Time.deltaTime;
        if (Timer - GameMode.gamemode.SpawnRate >= 0)
        {
            Timer = 0f;
            CreateNewGate();
        }
	}
    void CreateNewGate()
    {
        height = Random.Range(yMin, yMax);
        Gate = Instantiate(Prototype, new Vector2(RespawnPosition, height), Quaternion.identity);
        Gate.AddComponent<ScrollBackGround>();
        if (!GameMode.gamemode.gameover)
        {
            Destroy(Gate, DestroyTime);
        } 
    }

}
