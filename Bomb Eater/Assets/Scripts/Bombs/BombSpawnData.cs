using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single bomb spawn in an encounter
/// </summary>
[System.Serializable]
public class BombSpawnData
{
    public Bomb bombPrefab;
    public Vector3 position;
    public float delayBeforeSpawn = 0f;

    public BombSpawnData(Bomb prefab, Vector3 pos, float delay = 0f)
    {
        bombPrefab = prefab;
        position = pos;
        delayBeforeSpawn = delay;
    }
}
