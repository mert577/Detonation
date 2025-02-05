using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{



    public int score;
    
    public int bombsDefused;

    public int timeSurvived;

    public GameState gameState;
    public static GameManager instance;

    public UnityEvent OnGameOver = new UnityEvent();

    public float timePlayed;



    float startTime=0;
    float endTime=0;

    

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        gameState = GameState.OnTitleScreen;
        if (!PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }
    }

    private void Update()
    {

        if(score> PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        if(gameState == GameState.Playing)
        {
            timePlayed += Time.deltaTime;
        }
        else if(gameState== GameState.OnTitleScreen)
        {
            if (Input.anyKeyDown)
            {

                StartRun();
            }
        }

        score = (ProgressManager.instance.bombsDefused*10) +(int) timePlayed;
    }

    public void StartRun()
    {
        gameState = GameState.Playing;
        startTime = Time.time;
        Camera.main.transform.DOMoveY(0, 1f).SetEase(Ease.InOutQuad).OnComplete(() => {  BombSpawner.instance.StartSpawning(); UIManager.instance.ActivateHealthBar(); });
      
    }


    public void GameOver(){
        endTime = Time.time;
        timeSurvived = (int)(endTime - startTime);

        
        gameState = GameState.GameOver;

        OnGameOver.Invoke();

    }

    
}


public enum GameState
{
    Playing =0,
    WaitingForUpgrade=1,
    Paused=2,
    OnTitleScreen=3,
    GameOver=4,
}
