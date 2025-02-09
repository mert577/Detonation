using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class DeathPanel : MonoBehaviour
{


    public TextMeshProUGUI titleText;

    public TextMeshProUGUI timeSurvivedText;
    public TextMeshProUGUI bombsDefusedText;
    public TextMeshProUGUI scoreText;

    public RectTransform[] allElements;


    public float timeBetweenElements = 0.1f;

    public float punchScaleAmount =1.1f;

    public float animationTime = 0.5f;




    public void DeathPanelEnterAnimation(){
        timeSurvivedText.text = GameManager.instance.timeSurvived.ToString() +"s";
        bombsDefusedText.text = ProgressManager.instance.bombsDefused.ToString();
        scoreText.text = GameManager.instance.score.ToString();
        StartCoroutine(_());

        IEnumerator _(){

            foreach (var item in allElements)
            {
               item.DOPunchScale(Vector3.one * punchScaleAmount, animationTime, 1, 0); 
                yield return new WaitForSeconds(timeBetweenElements);
            }
        
        }
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
