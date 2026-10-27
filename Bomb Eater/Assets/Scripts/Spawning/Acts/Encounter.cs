using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A hand-placed group of bombs, spawned one after another.
/// </summary>
[System.Serializable]
public class Encounter
{
    public string encounterName;
    public List<BombSpawnData> bombSpawns = new List<BombSpawnData>();
    [Tooltip("Wait until every bomb of this encounter is defused or has exploded before moving on.")]
    public bool waitForBombClear;
    [Tooltip("Seconds to wait before the next encounter starts.")]
    public float delayBeforeNextEncounter = 2f;
}
