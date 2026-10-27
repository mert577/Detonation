using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The acts an <see cref="ActSpawner"/> plays, in order.
/// </summary>
[CreateAssetMenu(fileName = "EncounterConfiguration", menuName = "Bomb Eater/Encounter Configuration", order = 11)]
public class EncounterConfiguration : ScriptableObject
{
    public List<Act> acts = new List<Act>();

    [Tooltip("After the final act, start again from the first one. Otherwise the final act repeats.")]
    public bool loopActs = true;

    [Tooltip("Seconds before the first act starts.")]
    public float initialDelay = 1f;

    public Act GetAct(int index)
    {
        if (acts.Count == 0)
        {
            return null;
        }

        int actIndex = loopActs ? index % acts.Count : Mathf.Min(index, acts.Count - 1);
        return acts[actIndex];
    }
}
