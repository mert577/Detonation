using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Playing = 0,
    OnTitleScreen = 3,
    GameOver = 4,
}

/// <summary>
/// Runs the title → playing → game over flow and keeps the run's stats and score.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    const string HighscoreKey = "Highscore";

    [Tooltip("Decides when and where bombs appear. Drag a different WaveSource here to switch game mode.")]
    [SerializeField] WaveSource waveSource;
    [SerializeField] PlayerHealth player;

    [Header("Scoring")]
    [SerializeField] int pointsPerDefuse = 10;
    [SerializeField] float pointsPerSecond = 1f;

    [Header("Rewards")]
    [Tooltip("Heal one heart every time this many bombs have been defused. 0 turns healing off.")]
    [Min(0)] [SerializeField] int defusesPerHeal = 35;

    [Header("Intro")]
    [Tooltip("The camera starts above the arena on the title screen and moves here when a run starts.")]
    [SerializeField] float playCameraY;
    [SerializeField] float introDuration = 1f;

    [Tooltip("Extra hooks for when the run ends (music, effects...). The UI already listens to GameEvents.RunEnded.")]
    public UnityEvent OnGameOver = new UnityEvent();

    public GameState State { get; private set; } = GameState.OnTitleScreen;
    public WaveSource WaveSource => waveSource;
    public int Score { get; private set; }
    public int BombsDefused { get; private set; }
    public float TimePlayed { get; private set; }
    public int TimeSurvived => (int)TimePlayed;
    public int Highscore => Mathf.Max(savedHighscore, Score);
    public int DefusesUntilHeal => defusesPerHeal > 0 ? defusesPerHeal - BombsDefused % defusesPerHeal : 0;

    int savedHighscore;

    protected override void Awake()
    {
        base.Awake();
        if (instance != this)
        {
            return;
        }

        savedHighscore = PlayerPrefs.GetInt(HighscoreKey, 0);
        TimeScaleController.ResetTimeScale();
    }

    void OnEnable()
    {
        GameEvents.BombDefused += HandleBombDefused;
        GameEvents.PlayerDied += GameOver;
    }

    void OnDisable()
    {
        GameEvents.BombDefused -= HandleBombDefused;
        GameEvents.PlayerDied -= GameOver;
    }

    void Update()
    {
        if (State == GameState.OnTitleScreen && GameInput.AnyKeyPressed)
        {
            StartRun();
            return;
        }

        if (State == GameState.Playing)
        {
            TimePlayed += Time.deltaTime;
            UpdateScore(false);
        }
    }

    public void StartRun()
    {
        if (State != GameState.OnTitleScreen)
        {
            return;
        }

        State = GameState.Playing;
        Camera.main.transform.DOMoveY(playCameraY, introDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            if (waveSource != null)
            {
                waveSource.StartSpawning();
            }

            GameEvents.RaiseRunStarted();
        });
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
        {
            return;
        }

        State = GameState.GameOver;
        if (waveSource != null)
        {
            waveSource.StopSpawning();
        }

        if (Score > savedHighscore)
        {
            savedHighscore = Score;
            PlayerPrefs.SetInt(HighscoreKey, Score);
            PlayerPrefs.Save();
        }

        GameEvents.RaiseRunEnded();
        OnGameOver.Invoke();
    }

    void HandleBombDefused(Bomb bomb)
    {
        if (State != GameState.Playing)
        {
            return;
        }

        BombsDefused++;
        if (defusesPerHeal > 0 && BombsDefused % defusesPerHeal == 0 && player != null)
        {
            player.Heal();
        }

        UpdateScore(true);
    }

    void UpdateScore(bool forceNotify)
    {
        int score = BombsDefused * pointsPerDefuse + (int)(TimePlayed * pointsPerSecond);
        if (score == Score && !forceNotify)
        {
            return;
        }

        Score = score;
        GameEvents.RaiseScoreChanged(Score);
    }
}
