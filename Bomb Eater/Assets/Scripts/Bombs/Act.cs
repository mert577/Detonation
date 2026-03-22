using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an Act - an arena/level with its geometry and encounter sequence
/// </summary>
[System.Serializable]
public class Act
{
    public string actName;
    public GameObject arenaGeometryPrefab;
    public List<Encounter> encounters = new List<Encounter>();

    [Tooltip("Optional object to instantiate at act start (can be used for visual effects, etc)")]
    public GameObject actStartEffectPrefab;

    public Act()
    {
    }

    public Act(string name)
    {
        actName = name;
    }

    /// <summary>
    /// Returns total encounter count
    /// </summary>
    public int GetEncounterCount()
    {
        return encounters.Count;
    }
}
