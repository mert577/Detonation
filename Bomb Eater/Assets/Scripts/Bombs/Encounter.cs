using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an encounter - a sequence of bombs to spawn
/// </summary>
[System.Serializable]
public class Encounter
{
    public string encounterName;
    public List<BombSpawnData> bombSpawns = new List<BombSpawnData>();
    public float delayBeforeNextEncounter = 2f;

    [Tooltip("If true, encounter waits for all bombs from previous spawn to be defused before spawning next bomb")]
    public bool waitForBombClear = false;

    public Encounter()
    {
    }

    public Encounter(string name)
    {
        encounterName = name;
    }

    /// <summary>
    /// Returns how long this encounter takes to fully spawn all bombs (not including wait for clear)
    /// </summary>
    public float GetTotalSpawnTime()
    {
        float maxTime = 0f;
        foreach (var spawn in bombSpawns)
        {
            maxTime = Mathf.Max(maxTime, spawn.delayBeforeSpawn);
        }
        return maxTime;
    }
}
