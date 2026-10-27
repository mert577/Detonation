using System.Collections;
using UnityEngine;

/// <summary>
/// Places hazards in rings that grow outwards from the bomb: crosses, lines towards the player, circular waves.
/// </summary>
[CreateAssetMenu(fileName = "HazardRings", menuName = "Bomb Eater/Explosion Patterns/Hazard Rings")]
public class HazardRingsPattern : ExplosionPattern
{
    [Tooltip("Spawned at every point of every ring, e.g. a damage area or a still bullet.")]
    [SerializeField] GameObject hazard;
    [Tooltip("Number of rings, counting the one at the centre.")]
    [Min(1)] [SerializeField] int rings = 5;
    [Tooltip("Hazards per ring. 4 makes a +, 1 makes a single line.")]
    [Min(1)] [SerializeField] int arms = 4;
    [Tooltip("Extra hazards added to each ring further out. Use this for circular waves.")]
    [Min(0f)] [SerializeField] float armsAddedPerRing;
    [SerializeField] float ringSpacing = 1f;
    [SerializeField] float timeBetweenRings = .05f;
    [Tooltip("Point the first arm at the player instead of to the right.")]
    [SerializeField] bool aimAtPlayer;
    [SerializeField] float angleOffset;

    public override IEnumerator Play(ExplosionContext context)
    {
        Vector2 baseDirection = aimAtPlayer ? context.DirectionToTarget : Vector2.right;
        baseDirection = Quaternion.Euler(0f, 0f, angleOffset) * baseDirection;

        for (int ring = 0; ring < rings; ring++)
        {
            if (ring == 0)
            {
                context.Spawn(hazard, context.Origin);
            }
            else
            {
                int count = Mathf.CeilToInt(arms + ring * armsAddedPerRing);
                for (int arm = 0; arm < count; arm++)
                {
                    Vector2 direction = Quaternion.Euler(0f, 0f, 360f / count * arm) * baseDirection;
                    context.Spawn(hazard, context.Origin + direction * (ring * ringSpacing));
                }
            }

            if (ring < rings - 1)
            {
                yield return new WaitForSeconds(timeBetweenRings);
            }
        }
    }
}
