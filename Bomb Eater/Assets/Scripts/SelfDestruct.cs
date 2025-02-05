using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum AnimationType
{
    None,
    Scale,

    SpawnObject

}
public class SelfDestruct : MonoBehaviour
{

    public float selfDestructTime;
    public float animationTime;

    [SerializeField] AnimationType animationType;
    [SerializeField] GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (animationType == AnimationType.None)
        {

            Destroy(this.gameObject, selfDestructTime);
        }
        else if(animationType == AnimationType.Scale)
        {
          StartCoroutine(SelfDestructWithAnimation());
        }
        else if(animationType == AnimationType.SpawnObject)
        {
            StartCoroutine(SelfDestructSpawnObject());
        }
    }


    IEnumerator SelfDestructSpawnObject()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    IEnumerator SelfDestructWithAnimation()
    {
        yield return new WaitForSeconds(selfDestructTime);
        transform.DOScale(Vector3.zero, animationTime).SetEase(Ease.InOutBack);
        yield return new WaitForSeconds(animationTime);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
