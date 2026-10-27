using System.Collections;
using UnityEngine;

/// <summary>
/// Fires projectiles evenly around a circle, optionally several times in a row.
/// </summary>
[CreateAssetMenu(fileName = "RadialBurst", menuName = "Bomb Eater/Explosion Patterns/Radial Burst")]
public class RadialBurstPattern : ExplosionPattern
{
    [SerializeField] Projectile projectile;
    [Min(1)] [SerializeField] int projectilesPerBurst = 4;
    [Tooltip("Rotates the whole ring, in degrees. 45 turns a + into an x.")]
    [SerializeField] float angleOffset;
    [Tooltip("Point the first projectile at the player instead of to the right.")]
    [SerializeField] bool aimAtPlayer;
    [Min(1)] [SerializeField] int bursts = 1;
    [SerializeField] float timeBetweenBursts = .1f;
    [Tooltip("Extra rotation for every following burst, in degrees.")]
    [SerializeField] float rotationPerBurst;

    public override IEnumerator Play(ExplosionContext context)
    {
        float baseAngle = angleOffset + (aimAtPlayer ? context.AngleToTarget : 0f);
        float step = 360f / projectilesPerBurst;

        for (int burst = 0; burst < bursts; burst++)
        {
            for (int i = 0; i < projectilesPerBurst; i++)
            {
                context.LaunchProjectile(projectile, baseAngle + burst * rotationPerBurst + i * step);
            }

            if (burst < bursts - 1)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }
    }
}
