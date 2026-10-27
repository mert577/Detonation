using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns a single object, e.g. a lingering damage area, at the bomb or at the player.
/// </summary>
[CreateAssetMenu(fileName = "SpawnHazard", menuName = "Bomb Eater/Explosion Patterns/Spawn Hazard")]
public class SpawnHazardPattern : ExplosionPattern
{
    [SerializeField] GameObject hazard;
    [SerializeField] float delay;
    [Tooltip("Spawn where the player was when the bomb exploded instead of at the bomb.")]
    [SerializeField] bool atPlayer;

    public override IEnumerator Play(ExplosionContext context)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        context.Spawn(hazard, atPlayer ? context.TargetPosition : context.Origin);
    }
}
