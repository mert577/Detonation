using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for building encounters and acts programmatically
/// Useful if you want to generate encounters from code instead of hardcoding in inspector
/// </summary>
public class EncounterBuilder
{
    /// <summary>
    /// Create a new encounter with given name
    /// </summary>
    public static Encounter CreateEncounter(string name)
    {
        return new Encounter(name);
    }

    /// <summary>
    /// Add a bomb spawn to an encounter
    /// </summary>
    public static void AddBombSpawn(Encounter encounter, Bomb bombPrefab, Vector3 position, float delay = 0f)
    {
        encounter.bombSpawns.Add(new BombSpawnData(bombPrefab, position, delay));
    }

    /// <summary>
    /// Create a new act with given name
    /// </summary>
    public static Act CreateAct(string name, GameObject arenaGeometryPrefab = null)
    {
        return new Act(name) { arenaGeometryPrefab = arenaGeometryPrefab };
    }

    /// <summary>
    /// Add an encounter to an act
    /// </summary>
    public static void AddEncounter(Act act, Encounter encounter)
    {
        act.encounters.Add(encounter);
    }

    /// <summary>
    /// Create a grid of bombs (useful for pattern encounters)
    /// </summary>
    public static Encounter CreateGridEncounter(
        string name,
        Bomb bombPrefab,
        Vector3 gridCenter,
        int rows,
        int cols,
        float spacing,
        float staggerDelay = 0f
    )
    {
        Encounter encounter = new Encounter(name);
        float halfRows = (rows - 1) * spacing * 0.5f;
        float halfCols = (cols - 1) * spacing * 0.5f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 pos = gridCenter + new Vector3(
                    c * spacing - halfCols,
                    0,
                    r * spacing - halfRows
                );

                float delay = staggerDelay * (r * cols + c);
                AddBombSpawn(encounter, bombPrefab, pos, delay);
            }
        }

        return encounter;
    }

    /// <summary>
    /// Create circular pattern of bombs
    /// </summary>
    public static Encounter CreateCircleEncounter(
        string name,
        Bomb bombPrefab,
        Vector3 center,
        float radius,
        int bombCount,
        float staggerDelay = 0f
    )
    {
        Encounter encounter = new Encounter(name);
        float angleStep = 360f / bombCount;

        for (int i = 0; i < bombCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 pos = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            float delay = staggerDelay * i;
            AddBombSpawn(encounter, bombPrefab, pos, delay);
        }

        return encounter;
    }
}
