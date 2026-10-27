using System.Collections;
using UnityEngine;

/// <summary>
/// Fires one projectile per arm each step while the arms rotate, like a sprinkler.
/// </summary>
[CreateAssetMenu(fileName = "Sweep", menuName = "Bomb Eater/Explosion Patterns/Sweep")]
public class SweepPattern : ExplosionPattern
{
    [SerializeField] Projectile projectile;
    [Min(1)] [SerializeField] int steps = 6;
    [Tooltip("Degrees each arm turns over the whole sweep.")]
    [SerializeField] float sweepAngle = 180f;
    [SerializeField] float startAngle = 90f;
    [Tooltip("Point the start angle at the player.")]
    [SerializeField] bool aimAtPlayer;
    [Tooltip("Arms fired every step, spread evenly around the circle. 2 fires in opposite directions.")]
    [Min(1)] [SerializeField] int arms = 2;
    [SerializeField] float timeBetweenSteps = .1f;

    public override IEnumerator Play(ExplosionContext context)
    {
        float baseAngle = startAngle + (aimAtPlayer ? context.AngleToTarget : 0f);
        float stepAngle = sweepAngle / steps;
        float armAngle = 360f / arms;

        for (int step = 0; step < steps; step++)
        {
            for (int arm = 0; arm < arms; arm++)
            {
                context.LaunchProjectile(projectile, baseAngle + step * stepAngle + arm * armAngle);
            }

            if (step < steps - 1)
            {
                yield return new WaitForSeconds(timeBetweenSteps);
            }
        }
    }
}
