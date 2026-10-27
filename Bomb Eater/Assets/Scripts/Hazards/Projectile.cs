using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A bullet that flies straight and disappears on the first trigger it touches.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [FormerlySerializedAs("particles")]
    [SerializeField] ParticleSystem hitParticles;

    protected Rigidbody2D rb;
    protected float topSpeed;
    bool hit;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <param name="speedMultiplier">Scales the projectile's speed (difficulty, act settings).</param>
    /// <param name="launchSpeedMultiplier">Extra per-shot variation applied to the launch only.</param>
    public virtual void Launch(float angle, float speedMultiplier = 1f, float launchSpeedMultiplier = 1f)
    {
        topSpeed = speed * speedMultiplier;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        rb.linearVelocity = transform.right * topSpeed * launchSpeedMultiplier;
    }

    protected virtual void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, UtilityClass.GetRotationFromDirection(rb.linearVelocity));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit)
        {
            return;
        }

        hit = true;
        if (hitParticles != null)
        {
            Destroy(Instantiate(hitParticles, transform.position, Quaternion.identity).gameObject, 1f);
        }

        StartCoroutine(DestroyAtEndOfFrame());
    }

    // Waits so damage components on this object still get this frame's contact.
    IEnumerator DestroyAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
