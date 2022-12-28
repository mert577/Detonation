using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Saw : MonoBehaviour
{
    [SerializeField]
    List<Transform> wayPoints;
    public float time;


    public GameObject afterImage;

    public int afterImageCount;

    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToWayPoint(0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward* rotateSpeed*Time.deltaTime);
    }


    IEnumerator GoToWayPoint(int index)
    {
        index = index % wayPoints.Count;

        transform.DOMove(wayPoints[index].position, time).SetEase(Ease.InOutSine);
     //   StartCoroutine(SpawnAfterImages(afterImageCount));
        yield return new WaitForSeconds(time);
        StartCoroutine(GoToWayPoint(index+1));
    }

    IEnumerator SpawnAfterImages(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(afterImage, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(time / count);
        }

    }
}
