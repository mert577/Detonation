using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Encounter-based act spawner system. Independent from BombSpawner.
/// Spawns bombs according to designed encounters in acts.
/// </summary>
public class ActSpawner : MonoBehaviour
{
    [SerializeField] private EncounterConfiguration encounterConfig;
    [SerializeField] private Transform playerTransform;

    /// Encounter lifecycle events
    public delegate void EncounterEvent();
    public static event EncounterEvent OnEncounterStart;
    public static event EncounterEvent OnEncounterEnd;
    public static event EncounterEvent OnActStart;
    public static event EncounterEvent OnActEnd;

    private int currentActIndex = 0;
    private int currentEncounterIndex = 0;
    private bool spawning = false;
    private bool waitingForBombsClear = false;

    public static ActSpawner instance;

    // Current state properties
    public int CurrentActIndex => currentActIndex;
    public int CurrentEncounterIndex => currentEncounterIndex;
    public Act CurrentAct => encounterConfig?.GetAct(currentActIndex);
    public Encounter CurrentEncounter => CurrentAct?.encounters.Count > currentEncounterIndex ? CurrentAct.encounters[currentEncounterIndex] : null;
    public bool IsSpawning => spawning;

    private GameObject currentArenaInstance;
    private List<Bomb> activeBombs = new List<Bomb>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        CachePlayerTransform();

        if (encounterConfig == null)
        {
            Debug.LogError("ActSpawner: No EncounterConfiguration assigned!");
            return;
        }

        // Optional: Start spawning immediately
        // StartSpawning();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void CachePlayerTransform()
    {
        if (playerTransform != null)
            return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    public Transform PlayerTransform => playerTransform;

    /// <summary>
    /// Start spawning acts and encounters
    /// </summary>
    public void StartSpawning()
    {
        if (spawning)
        {
            Debug.LogWarning("ActSpawner: Already spawning!");
            return;
        }

        spawning = true;
        currentActIndex = 0;
        currentEncounterIndex = 0;
        CachePlayerTransform();
        StartCoroutine(SpawnActsCoroutine());
    }

    /// <summary>
    /// Stop spawning and clean up
    /// </summary>
    public void StopSpawning()
    {
        spawning = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// Skip to next act (useful for testing/debugging)
    /// </summary>
    public void SkipToNextAct()
    {
        currentActIndex++;
        currentEncounterIndex = 0;
    }

    /// <summary>
    /// Main coroutine that loops through acts
    /// </summary>
    private IEnumerator SpawnActsCoroutine()
    {
        yield return new WaitForSeconds(encounterConfig.initialDelay);

        while (spawning)
        {
            yield return StartCoroutine(SpawnActCoroutine());
            currentActIndex++;
        }
    }

    /// <summary>
    /// Spawn a full act with all its encounters
    /// </summary>
    private IEnumerator SpawnActCoroutine()
    {
        Act act = encounterConfig.GetAct(currentActIndex);

        if (act == null)
        {
            Debug.LogWarning("ActSpawner: No act found at index " + currentActIndex);
            yield break;
        }

        Debug.Log($"[ActSpawner] Starting Act: {act.actName}");

        // Instantiate arena geometry
        if (act.arenaGeometryPrefab != null)
        {
            if (currentArenaInstance != null)
            {
                Destroy(currentArenaInstance);
            }
            currentArenaInstance = Instantiate(act.arenaGeometryPrefab);
        }

        // Spawn act start effect if any
        if (act.actStartEffectPrefab != null)
        {
            Instantiate(act.actStartEffectPrefab);
        }

        OnActStart?.Invoke();

        currentEncounterIndex = 0;

        // Spawn each encounter in this act
        for (int i = 0; i < act.encounters.Count; i++)
        {
            if (!spawning)
                yield break;

            currentEncounterIndex = i;
            yield return StartCoroutine(SpawnEncounterCoroutine(act.encounters[i]));
        }

        OnActEnd?.Invoke();
        Debug.Log($"[ActSpawner] Finished Act: {act.actName}");
    }

    /// <summary>
    /// Spawn a single encounter
    /// </summary>
    private IEnumerator SpawnEncounterCoroutine(Encounter encounter)
    {
        Debug.Log($"[ActSpawner] Starting Encounter: {encounter.encounterName}");
        OnEncounterStart?.Invoke();

        activeBombs.Clear();

        // Spawn all bombs in this encounter
        foreach (var bombSpawn in encounter.bombSpawns)
        {
            if (!spawning)
                yield break;

            yield return new WaitForSeconds(bombSpawn.delayBeforeSpawn);

            Bomb bomb = Instantiate(
                bombSpawn.bombPrefab,
                bombSpawn.position,
                Quaternion.identity
            );

            if (playerTransform != null)
            {
                bomb.SetPlayerTransform(playerTransform);
            }

            activeBombs.Add(bomb);
        }

        // Optional: Wait for all bombs in encounter to be cleared
        if (encounter.waitForBombClear)
        {
            waitingForBombsClear = true;
            while (waitingForBombsClear && activeBombs.Count > 0)
            {
                // Remove null bombs (defused or exploded)
                activeBombs.RemoveAll(b => b == null || !b.isActiveAndEnabled);
                
                if (activeBombs.Count == 0)
                {
                    waitingForBombsClear = false;
                    break;
                }

                yield return null;
            }
        }

        // Wait before next encounter
        yield return new WaitForSeconds(encounter.delayBeforeNextEncounter);

        OnEncounterEnd?.Invoke();
        Debug.Log($"[ActSpawner] Finished Encounter: {encounter.encounterName}");
    }

    /// <summary>
    /// Called by bomb when it's defused - updates active bomb list
    /// </summary>
    public void OnBombDefused(Bomb bomb)
    {
        activeBombs.Remove(bomb);
    }
}
