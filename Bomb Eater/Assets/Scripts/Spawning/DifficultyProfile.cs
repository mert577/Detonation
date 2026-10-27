using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Balancing for endless mode: wave rhythm plus difficulty tiers that unlock by wave number.
/// Tune or add tiers here instead of in code.
/// </summary>
[CreateAssetMenu(fileName = "EndlessDifficulty", menuName = "Bomb Eater/Endless Difficulty Profile", order = 10)]
public class DifficultyProfile : ScriptableObject
{
    [Header("Wave rhythm")]
    [Tooltip("Seconds between waves before the tier and rush multipliers are applied.")]
    public float baseWaveInterval = 2.6f;
    [Tooltip("Random +/- seconds added to every wave interval.")]
    public float waveIntervalJitter = .2f;
    [Tooltip("Random seconds between bombs of the same wave.")]
    public Vector2 bombSpawnStagger = new Vector2(.1f, .3f);

    [Header("Rush waves")]
    [Tooltip("Waves are grouped in cycles of this length; the last waves of each cycle come faster.")]
    [Min(1)] public int rushCycleLength = 7;
    [Min(0)] public int rushWavesPerCycle = 2;
    public float rushIntervalMultiplier = .75f;

    [Header("Tiers")]
    [Tooltip("The tier with the highest start wave that has been reached is used.")]
    public List<DifficultyTier> tiers = new List<DifficultyTier>();

    public DifficultyTier GetTier(int wave)
    {
        DifficultyTier reached = null;
        DifficultyTier earliest = null;
        foreach (DifficultyTier tier in tiers)
        {
            if (earliest == null || tier.startWave < earliest.startWave)
            {
                earliest = tier;
            }

            if (wave >= tier.startWave && (reached == null || tier.startWave >= reached.startWave))
            {
                reached = tier;
            }
        }

        return reached ?? earliest;
    }

    public bool IsRushWave(int wave)
    {
        return wave % rushCycleLength >= rushCycleLength - rushWavesPerCycle;
    }

    public float GetWaveInterval(int wave)
    {
        DifficultyTier tier = GetTier(wave);
        float tierMultiplier = tier != null ? tier.waveIntervalMultiplier : 1f;
        float rushMultiplier = IsRushWave(wave) ? rushIntervalMultiplier : 1f;
        return baseWaveInterval * tierMultiplier * rushMultiplier + Random.Range(-waveIntervalJitter, waveIntervalJitter);
    }
}

[System.Serializable]
public class DifficultyTier
{
    public string name = "Tier";
    [Tooltip("First wave (counting from 0) that uses this tier.")]
    [Min(0)] public int startWave;
    [Tooltip("Inclusive min and max number of bombs per wave.")]
    public Vector2Int bombsPerWave = new Vector2Int(3, 5);
    [Range(0f, 1f)] public float movementChance;
    public float projectileSpeedMultiplier = 1f;
    public float waveIntervalMultiplier = 1f;
    public bool missilesEnabled;
    public List<WeightedBomb> bombPool = new List<WeightedBomb>();

    public BombModifiers Modifiers => new BombModifiers(movementChance, projectileSpeedMultiplier);

    public int RollBombCount()
    {
        return Random.Range(bombsPerWave.x, Mathf.Max(bombsPerWave.x, bombsPerWave.y) + 1);
    }

    public BombDefinition PickBomb()
    {
        float totalWeight = 0f;
        int validCount = 0;
        foreach (WeightedBomb entry in bombPool)
        {
            if (entry.bomb == null)
            {
                continue;
            }

            totalWeight += Mathf.Max(0f, entry.weight);
            validCount++;
        }

        if (validCount == 0)
        {
            return null;
        }

        // All weights at zero means "not tuned yet": pick uniformly instead of never spawning.
        bool uniform = totalWeight <= 0f;
        float roll = Random.value * (uniform ? validCount : totalWeight);
        BombDefinition last = null;
        foreach (WeightedBomb entry in bombPool)
        {
            if (entry.bomb == null)
            {
                continue;
            }

            last = entry.bomb;
            roll -= uniform ? 1f : Mathf.Max(0f, entry.weight);
            if (roll < 0f)
            {
                return entry.bomb;
            }
        }

        return last;
    }
}

[System.Serializable]
public class WeightedBomb
{
    public BombDefinition bomb;
    [Tooltip("Relative chance compared to the other bombs in the pool.")]
    [Min(0f)] public float weight = 1f;
}
