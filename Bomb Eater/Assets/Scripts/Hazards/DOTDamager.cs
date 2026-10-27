using System.Collections;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Lingering damage area: grows in, emits particles for its lifetime, shrinks out, then destroys itself.
/// Pair it with a Damager for the damage and a Rotate for the spin.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class DOTDamager : MonoBehaviour
{
    [SerializeField] float lifetime = 2.5f;
    [SerializeField] float growTime = .1f;
    [SerializeField] float shrinkTime = .2f;

    ParticleSystem particles;
    Vector3 originalScale;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        originalScale = transform.localScale;
    }

    IEnumerator Start()
    {
        ParticleSystem.EmissionModule emission = particles.emission;

        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, growTime).SetEase(Ease.InQuart).SetLink(gameObject);
        yield return new WaitForSeconds(growTime);

        emission.enabled = true;
        yield return new WaitForSeconds(lifetime);
        emission.enabled = false;

        transform.DOScale(Vector3.zero, shrinkTime).SetEase(Ease.InQuart).SetLink(gameObject);
        yield return new WaitForSeconds(shrinkTime);
        Destroy(gameObject);
    }
}
