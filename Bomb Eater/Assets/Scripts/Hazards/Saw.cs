using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Travels between waypoints forever. Pair it with a Damager for damage and a Rotate for the spin.
/// </summary>
public class Saw : MonoBehaviour
{
    [SerializeField] List<Transform> wayPoints;
    [Tooltip("Seconds to travel from one waypoint to the next.")]
    [SerializeField] float time = 1f;

    IEnumerator Start()
    {
        if (wayPoints.Count == 0)
        {
            yield break;
        }

        for (int index = 0; ; index = (index + 1) % wayPoints.Count)
        {
            transform.DOMove(wayPoints[index].position, time).SetEase(Ease.InOutSine).SetLink(gameObject);
            yield return new WaitForSeconds(time);
        }
    }
}
