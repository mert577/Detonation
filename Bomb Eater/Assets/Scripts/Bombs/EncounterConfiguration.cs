using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object that holds all Acts and Encounters for the game
/// Assign this in the Inspector for the BombSpawner to use
/// </summary>
[CreateAssetMenu(fileName = "EncounterConfiguration", menuName = "Game/Encounter Configuration", order = 1)]
public class EncounterConfiguration : ScriptableObject
{
    public List<Act> acts = new List<Act>();

    [Tooltip("If true, loops back to Act 0 after the final act")]
    public bool loopActs = true;

    [Tooltip("Delay before starting the first act")]
    public float initialDelay = 1f;

    /// <summary>
    /// Get act at index, with looping support
    /// </summary>
    public Act GetAct(int index)
    {
        if (acts.Count == 0)
            return null;

        int loopedIndex = loopActs ? index % acts.Count : Mathf.Min(index, acts.Count - 1);
        return acts[loopedIndex];
    }

    public int GetActCount()
    {
        return acts.Count;
    }

    /// <summary>
    /// Get total bomb count across all acts (for debugging)
    /// </summary>
    public int GetTotalBombCount()
    {
        int total = 0;
        foreach (var act in acts)
        {
            foreach (var encounter in act.encounters)
            {
                total += encounter.bombSpawns.Count;
            }
        }
        return total;
    }
}
