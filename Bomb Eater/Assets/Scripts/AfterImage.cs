using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImage : MonoBehaviour
{

    public float afterImageDelay;
    public float afterImageTime;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AfterImageSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AfterImageSequence()
    {
        yield return new WaitForSeconds(afterImageDelay);
        GetComponent<SpriteRenderer>().DOFade(0f, afterImageTime);
        yield return new WaitForSeconds(afterImageTime);
       
        Destroy(this.gameObject);
    }
}
