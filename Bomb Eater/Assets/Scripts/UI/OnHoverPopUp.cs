using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OnHoverPopUp : MonoBehaviour
{
    [SerializeField] float punchScaleAmount = 1.1f;

    float originalScale;

    [SerializeField]
    float animationTime = 0.25f;


    void Start()
    {
        originalScale = transform.localScale.x;
    }
   public void ScaleUpBounce(){
       transform.DOScale(Vector3.one * punchScaleAmount, animationTime).SetEase(Ease.OutBounce);
   }

   public void ScaleBack(){
       transform.DOScale(Vector3.one * originalScale, animationTime     ).SetEase(Ease.OutBounce);
   }
}
