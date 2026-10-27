using UnityEngine;

/// <summary>
/// A projectile that steers towards the player, never faster than its top speed.
/// </summary>
public class HomingProjectile : Projectile
{
    [SerializeField] float accelerationForce;

    Transform target;

    public override void Launch(float angle, float speedMultiplier = 1f, float launchSpeedMultiplier = 1f)
    {
        target = PlayerLocator.Transform;
        base.Launch(angle, speedMultiplier, launchSpeedMultiplier);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 toTarget = ((Vector2)target.position - rb.position).normalized;
        rb.AddForce(accelerationForce * toTarget);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, topSpeed);
    }
}
