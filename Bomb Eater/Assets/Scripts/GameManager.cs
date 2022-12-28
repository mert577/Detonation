using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{



    public int Score;

    public GameState gameState;
    public static GameManager instance;

    public float timePlayed;

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

        if(Score> PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", Score);
        }

        if(gameState == GameState.Playing)
        {
            timePlayed += Time.deltaTime;
        }
        else if(gameState== GameState.OnTitleScreen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                StartRun();
            }
        }

        Score = (ProgressManager.instance.bombsDefused*10) +(int) timePlayed;
    }

    public void StartRun()
    {
        gameState = GameState.Playing;
        Camera.main.transform.DOMoveY(0, 1f).SetEase(Ease.InOutQuad).OnComplete(() => {  BombSpawner.instance.StartSpawning(); UIManager.instance.ActivateHealthBar(); });
      
    }

    
}


public enum GameState
{
    Playing =0,
    WaitingForUpgrade=1,
    Paused=2,
    OnTitleScreen=3,
}
