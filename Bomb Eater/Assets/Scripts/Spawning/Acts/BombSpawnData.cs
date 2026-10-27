using UnityEngine;

/// <summary>
/// One bomb inside an encounter.
/// </summary>
[System.Serializable]
public class BombSpawnData
{
    public BombDefinition bomb;
    [Tooltip("World position. Ignored when Random Position is on.")]
    public Vector2 position;
    public bool randomPosition;
    [Tooltip("Seconds to wait after the previous bomb of this encounter.")]
    public float delayBeforeSpawn;
    [Tooltip("Make the bomb wander around (if its definition allows movement).")]
    public bool moving;
}
