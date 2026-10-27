using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Endless mode: random waves whose size, bomb pool and pace come from a <see cref="DifficultyProfile"/>.
/// </summary>
public class EndlessWaveSpawner : WaveSource
{
    [SerializeField] DifficultyProfile difficulty;
    [SerializeField] BombSpawnService spawnService;
    [Tooltip("Optional. Turned on and off by the difficulty tiers.")]
    [SerializeField] MissileSpawner missileSpawner;
    [Tooltip("Bombs land at random points inside this area.")]
    [FormerlySerializedAs("bounds")]
    [SerializeField] Bounds spawnArea = new Bounds(Vector3.zero, new Vector3(15.2f, 15f, 0f));

    public int WaveNumber { get; private set; }
    public DifficultyTier CurrentTier { get; private set; }

    public override string WaveLabel => $"Wave {WaveNumber}";
    public override float WaveProgress => waveInterval > 0f ? Mathf.Clamp01(intervalElapsed / waveInterval) : 0f;

    Coroutine spawnLoop;
    float waveInterval;
    float intervalElapsed;

    public override void StartSpawning()
    {
        if (spawnLoop != null)
        {
            return;
        }

        if (difficulty == null || difficulty.tiers.Count == 0)
        {
            Debug.LogError("EndlessWaveSpawner: assign a DifficultyProfile with at least one tier.", this);
            return;
        }

        spawnLoop = StartCoroutine(SpawnLoop());
    }

    public override void StopSpawning()
    {
        if (spawnLoop != null)
        {
            StopCoroutine(spawnLoop);
            spawnLoop = null;
        }

        if (missileSpawner != null)
        {
            missileSpawner.StopSpawning();
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            int waveIndex = WaveNumber;
            DifficultyTier tier = difficulty.GetTier(waveIndex);
            ApplyTier(tier);

            waveInterval = difficulty.GetWaveInterval(waveIndex);
            intervalElapsed = 0f;
            WaveNumber++;
            GameEvents.RaiseWaveStarted(WaveNumber);

            int bombCount = tier.RollBombCount();
            for (int i = 0; i < bombCount; i++)
            {
                Vector2 position = UtilityClass.GetRandomPointInBounds(spawnArea);
                spawnService.Spawn(tier.PickBomb(), position, tier.Modifiers);
                yield return new WaitForSeconds(Random.Range(difficulty.bombSpawnStagger.x, difficulty.bombSpawnStagger.y));
            }

            // The interval counts from the moment the last bomb of the wave has landed.
            while (intervalElapsed < waveInterval)
            {
                intervalElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    void ApplyTier(DifficultyTier tier)
    {
        if (tier == CurrentTier)
        {
            return;
        }

        CurrentTier = tier;
        if (missileSpawner == null)
        {
            return;
        }

        missileSpawner.SpeedMultiplier = tier.projectileSpeedMultiplier;
        if (tier.missilesEnabled)
        {
            missileSpawner.StartSpawning();
        }
        else
        {
            missileSpawner.StopSpawning();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, .5f, 0f, .6f);
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
}
