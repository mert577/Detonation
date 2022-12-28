using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{


    [SerializeField] Image image;
    private void Update()
    {
       image.fillAmount = (BombSpawner.instance.waitTime -BombSpawner.instance.timer) / BombSpawner.instance.waitTime ;
    }
}
