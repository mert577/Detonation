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

    public


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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText(); 
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score:\n" + GameManager.instance.Score;
        highScoreText.text = "Highscore:\n" + PlayerPrefs.GetInt("Highscore");
        waveText.text = "Wave " + ProgressManager.instance.waveNumber;
        healText.text = "Bombs Until Heal:\n" + (35-(ProgressManager.instance.bombsDefused % 35));
    }


    public void ActivateHealthBar()
    {
        healthBar.SetActive(true);
    }
}
