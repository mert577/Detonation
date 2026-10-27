using UnityEngine;

/// <summary>
/// What a pattern knows about the explosion it belongs to, plus helpers to spawn things from it.
/// The target position is captured once, at the moment of the explosion.
/// </summary>
public class ExplosionContext
{
    public Vector2 Origin { get; }
    public Vector2 TargetPosition { get; }
    public float ProjectileSpeedMultiplier { get; }

    public ExplosionContext(Vector2 origin, Vector2 targetPosition, float projectileSpeedMultiplier)
    {
        Origin = origin;
        TargetPosition = targetPosition;
        ProjectileSpeedMultiplier = projectileSpeedMultiplier;
    }

    public Vector2 DirectionToTarget
    {
        get
        {
            Vector2 direction = TargetPosition - Origin;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector2.right;
        }
    }

    public float AngleToTarget => UtilityClass.GetRotationFromDirection(DirectionToTarget);

    /// <param name="launchSpeedMultiplier">Per-shot speed variation; homing projectiles keep their normal top speed.</param>
    public Projectile LaunchProjectile(Projectile prefab, float angle, float launchSpeedMultiplier = 1f)
    {
        if (prefab == null)
        {
            return null;
        }

        Projectile projectile = Object.Instantiate(prefab, Origin, Quaternion.identity);
        projectile.Launch(angle, ProjectileSpeedMultiplier, launchSpeedMultiplier);
        return projectile;
    }

    public GameObject Spawn(GameObject prefab, Vector2 position)
    {
        return prefab != null ? Object.Instantiate(prefab, position, Quaternion.identity) : null;
    }
}
