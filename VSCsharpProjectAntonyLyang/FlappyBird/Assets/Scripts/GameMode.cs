using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {
    public static GameMode gamemode;
    public float GameSpeed = 5f;
    public float  SpawnRate = 2f;
    public float SpeedStage = 0.5f;
    public Text ScoreDisplay;
    public GameObject GameOverUI;
    public GameObject NormalUI;
    public GameObject UpperGate;
    public float GateWidth;
    public Text GameOverText;
    public bool gameover = false;
    int Score;
    CharacterBehavior character;
    // Use this for initialization
    private void Awake()
    {
        gamemode = this;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
    public void ScoreUp()
    {
        Score += 1;
        print(Score);
        AddDifficulty();
        ScoreDisplay.text = "SCORE:" + Score.ToString();
    }
    void Accelerate()
    {
        GameSpeed += SpeedStage;
    }
    public void GameOver()
    {
        //Time.timeScale = 0;
        GameSpeed = 0f;
        gameover = true;
        NormalUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameOverText.text="YOUR LAST SCORE:" + Score.ToString();
    }
    public void AddDifficulty()
    {
        if (Score > 0 && Score % 5 == 0)
        {
            if (SpawnRate > 1f)
            {
                SpawnRate -= 0.2f;
            }
            Accelerate();
            if (UpperGate.transform.localPosition.y >= 3.8f)
            {
                var pos = UpperGate.transform.localPosition;
                pos.y -= GateWidth;
                UpperGate.transform.localPosition = pos;
            }
        }
    }
}
