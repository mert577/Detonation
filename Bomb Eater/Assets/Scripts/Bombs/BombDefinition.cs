using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Everything that makes one bomb type different from another. All bombs share one prefab;
/// a new bomb is a new asset (Create > Bomb Eater > Bomb Definition), not new code.
/// </summary>
[CreateAssetMenu(fileName = "Bomb_New", menuName = "Bomb Eater/Bomb Definition", order = 0)]
public class BombDefinition : ScriptableObject
{
    [Header("Look")]
    public Sprite sprite;
    public Color tint = Color.white;
    [Tooltip("Optional. Spawn this prefab instead of the shared bomb prefab (for bombs that need extra visuals).")]
    public Bomb prefabOverride;

    [Header("Fuse")]
    [Min(0.1f)] public float fuseTime = 2f;
    [Tooltip("Seconds before the explosion when the bomb starts shaking and flashing white.")]
    [Min(0f)] public float warningTime = 1f;

    [Header("Movement")]
    [Tooltip("If off, the bomb never moves, whatever the difficulty's movement chance is.")]
    public bool canMove = true;
    public float moveSpeed = 5f;

    [Header("Explosion")]
    [Tooltip("Played together when the bomb explodes. Mix any patterns (bullets, hazards, ...).")]
    public List<ExplosionPattern> patterns = new List<ExplosionPattern>();
    public GameObject explodeParticle;
    public ShakeSettings explodeShake = new ShakeSettings(.25f, .3f);

    [Header("Defuse")]
    public GameObject defuseParticle;
    public ShakeSettings defuseShake = new ShakeSettings(.1f, .5f);

    void OnValidate()
    {
        if (patterns.Count == 0)
        {
            Debug.LogWarning($"Bomb definition '{name}' has no explosion patterns, it will explode harmlessly.", this);
        }
    }
}
