using System.Collections;
using DG.Tweening;
using UnityEngine;

public enum AnimationType
{
    None,
    Scale,
    SpawnObject,
    Fade,
}

/// <summary>
/// Destroys its GameObject after a delay, optionally shrinking, fading or spawning something first.
/// </summary>
public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float selfDestructTime;
    [Tooltip("Length of the Scale or Fade animation.")]
    [SerializeField] float animationTime;
    [SerializeField] AnimationType animationType;
    [Tooltip("Spawned in place of this object when the animation type is Spawn Object.")]
    [SerializeField] GameObject objectToSpawn;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(selfDestructTime);

        switch (animationType)
        {
            case AnimationType.Scale:
                transform.DOScale(Vector3.zero, animationTime).SetEase(Ease.InOutBack).SetLink(gameObject);
                yield return new WaitForSeconds(animationTime);
                break;
            case AnimationType.Fade:
                if (TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.DOFade(0f, animationTime).SetLink(gameObject);
                }

                yield return new WaitForSeconds(animationTime);
                break;
            case AnimationType.SpawnObject:
                if (objectToSpawn != null)
                {
                    Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                }

                break;
        }

        Destroy(gameObject);
    }
}
