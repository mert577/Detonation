using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// HUD texts, title animation and screen switching. Updates only when GameEvents report a change.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] TextMeshPro highScoreText;
    [SerializeField] TextMeshPro waveText;
    [SerializeField] TextMeshPro healText;
    [Tooltip("{0} is replaced by the value.")]
    [TextArea(1, 3)] [SerializeField] string scoreFormat = "Score:\n{0}";
    [TextArea(1, 3)] [SerializeField] string highscoreFormat = "Highscore:\n{0}";
    [TextArea(1, 3)] [SerializeField] string healFormat = "Bombs Until Heal:\n{0}";

    [Header("Title")]
    [SerializeField] TextMeshPro titleText;
    [SerializeField] TextMeshPro playPromptText;
    [SerializeField] float titleLoopTime;
    [SerializeField] float titleScaleAmount;

    [Header("Screens")]
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject leaderboardScreen;
    [SerializeField] GameObject healthBar;

    void OnEnable()
    {
        GameEvents.ScoreChanged += HandleScoreChanged;
        GameEvents.WaveStarted += HandleWaveStarted;
        GameEvents.RunStarted += ActivateHealthBar;
        GameEvents.RunEnded += ActivateDeathScreen;
    }

    void OnDisable()
    {
        GameEvents.ScoreChanged -= HandleScoreChanged;
        GameEvents.WaveStarted -= HandleWaveStarted;
        GameEvents.RunStarted -= ActivateHealthBar;
        GameEvents.RunEnded -= ActivateDeathScreen;
    }

    void Start()
    {
        healthBar.SetActive(false);
        AnimateTitleText();
        HandleScoreChanged(GameManager.instance.Score);
        HandleWaveStarted(0);
    }

    public void AnimateTitleText()
    {
        titleText.transform.DOScale(titleScaleAmount, titleLoopTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetLink(titleText.gameObject);
        playPromptText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine).SetLink(playPromptText.gameObject);
    }

    void HandleScoreChanged(int score)
    {
        GameManager game = GameManager.instance;
        scoreText.text = string.Format(scoreFormat, score);
        highScoreText.text = string.Format(highscoreFormat, game.Highscore);
        healText.text = string.Format(healFormat, game.DefusesUntilHeal);
    }

    void HandleWaveStarted(int wave)
    {
        WaveSource source = GameManager.instance.WaveSource;
        waveText.text = source != null ? source.WaveLabel : string.Empty;
    }

    public void ActivateLeaderboardScreen()
    {
        leaderboardScreen.SetActive(true);
    }

    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void ActivateHealthBar()
    {
        healthBar.SetActive(true);
    }
}
