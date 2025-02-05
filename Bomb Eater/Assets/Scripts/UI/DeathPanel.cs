using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathPanel : MonoBehaviour
{


    public TextMeshProUGUI titleText;

    public TextMeshProUGUI timeSurvivedText;
    public TextMeshProUGUI bombsDefusedText;
    public TextMeshProUGUI scoreText;




    public void DeathPanelEnterAnimation(){
        timeSurvivedText.text = GameManager.instance.timeSurvived.ToString() +"s";
        bombsDefusedText.text = ProgressManager.instance.bombsDefused.ToString();
        scoreText.text = GameManager.instance.score.ToString();


    }




    // Start is called before the first frame update
    void Start()
    {
        DeathPanelEnterAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
