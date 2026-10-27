using System.Collections;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Warning marker that blinks for a moment, then launches the actual missile projectile.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Missile : MonoBehaviour
{
    [SerializeField] Projectile missile;
    [SerializeField] float warningTime = 1.5f;
    [SerializeField] int warningFlashes = 8;

    Vector2 direction = Vector2.right;
    float speedMultiplier = 1f;

    public void Init(Vector2 flightDirection, float projectileSpeedMultiplier)
    {
        direction = flightDirection.normalized;
        speedMultiplier = projectileSpeedMultiplier;
    }

    IEnumerator Start()
    {
        GetComponent<SpriteRenderer>().DOFade(0f, warningTime).SetEase(Ease.Flash, warningFlashes).SetLink(gameObject);
        yield return new WaitForSeconds(warningTime);

        Projectile projectile = Instantiate(missile, transform.position, Quaternion.identity);
        projectile.Launch(UtilityClass.GetRotationFromDirection(direction), speedMultiplier);
        Destroy(gameObject);
    }
}
