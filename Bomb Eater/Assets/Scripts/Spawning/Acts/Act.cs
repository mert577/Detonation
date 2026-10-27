using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An arena with its own sequence of encounters.
/// </summary>
[System.Serializable]
public class Act
{
    public string actName;
    [Tooltip("Optional. Spawned when the act starts; replaces the previous act's arena.")]
    public GameObject arenaGeometryPrefab;
    [Tooltip("Optional. Spawned once when the act starts (banners, effects...).")]
    public GameObject actStartEffectPrefab;
    [Tooltip("Speed multiplier for every projectile fired by this act's bombs.")]
    public float projectileSpeedMultiplier = 1f;
    public List<Encounter> encounters = new List<Encounter>();
}
