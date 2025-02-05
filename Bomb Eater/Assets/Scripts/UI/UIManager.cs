using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UIManager : MonoBehaviour
{


    public TextMeshPro scoreText;
    public TextMeshPro highScoreText;
    public TextMeshPro waveText;
    public TextMeshPro healText;

    public TextMeshPro playPromptText;

    [SerializeField] TextMeshPro titleText;
    [SerializeField] float titleLoopTime;
    [SerializeField] float titleScaleAmount;

    [SerializeField] GameObject deathScreen;


    public GameObject healthBar;

    public static UIManager instance;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetActive(false);
        AnimateTitleText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText(); 
    }

    
    public void AnimateTitleText(){
        titleText.transform.DOScale(titleScaleAmount, titleLoopTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        //fade in out play prompt text
        
        playPromptText.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score:\n" + GameManager.instance.score;
        highScoreText.text = "Highscore:\n" + PlayerPrefs.GetInt("Highscore");
        waveText.text = "Wave " + ProgressManager.instance.waveNumber;
        healText.text = "Bombs Until Heal:\n" + (35-(ProgressManager.instance.bombsDefused % 35));
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
