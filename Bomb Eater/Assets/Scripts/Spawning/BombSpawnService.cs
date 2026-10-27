using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates bombs from definitions and keeps track of the ones still in play. Every wave source spawns through this.
/// </summary>
public class BombSpawnService : MonoBehaviour
{
    [Tooltip("Used for every definition that has no prefab override.")]
    [SerializeField] Bomb defaultBombPrefab;

    readonly List<Bomb> activeBombs = new List<Bomb>();

    public int ActiveBombCount
    {
        get
        {
            activeBombs.RemoveAll(bomb => bomb == null || bomb.IsResolved);
            return activeBombs.Count;
        }
    }

    public Bomb Spawn(BombDefinition definition, Vector2 position, BombModifiers modifiers)
    {
        if (definition == null)
        {
            return null;
        }

        Bomb prefab = definition.prefabOverride != null ? definition.prefabOverride : defaultBombPrefab;
        Bomb bomb = Instantiate(prefab, position, Quaternion.identity);
        bomb.name = definition.name;
        bomb.Init(definition, modifiers);

        activeBombs.Add(bomb);
        GameEvents.RaiseBombSpawned(bomb);
        return bomb;
    }
}
