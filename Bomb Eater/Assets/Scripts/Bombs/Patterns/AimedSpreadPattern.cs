using System.Collections;
using UnityEngine;

/// <summary>
/// Fires a fan of projectiles centred on the player's direction.
/// </summary>
[CreateAssetMenu(fileName = "AimedSpread", menuName = "Bomb Eater/Explosion Patterns/Aimed Spread")]
public class AimedSpreadPattern : ExplosionPattern
{
    [SerializeField] Projectile projectile;
    [Min(1)] [SerializeField] int projectileCount = 3;
    [Tooltip("Degrees between neighbouring projectiles.")]
    [SerializeField] float spreadAngle = 30f;
    [Tooltip("Added to the aim direction, in degrees. 180 fires away from the player.")]
    [SerializeField] float angleOffset;
    [Tooltip("Random +/- fraction applied to each projectile's spread.")]
    [Min(0f)] [SerializeField] float spreadRandomness;
    [Tooltip("Random +/- fraction applied to each projectile's speed.")]
    [Min(0f)] [SerializeField] float speedRandomness;

    public override IEnumerator Play(ExplosionContext context)
    {
        float aimAngle = context.AngleToTarget + angleOffset;
        float centre = (projectileCount - 1) * .5f;

        for (int i = 0; i < projectileCount; i++)
        {
            float spread = spreadAngle * (1f + Random.Range(-spreadRandomness, spreadRandomness));
            float speed = 1f + Random.Range(-speedRandomness, speedRandomness);
            context.LaunchProjectile(projectile, aimAngle + (i - centre) * spread, speed);
        }

        yield break;
    }
}
