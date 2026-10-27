using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Designed mode: plays the acts and encounters of an <see cref="EncounterConfiguration"/> in order.
/// </summary>
public class ActSpawner : WaveSource
{
    [SerializeField] EncounterConfiguration encounterConfig;
    [SerializeField] BombSpawnService spawnService;
    [Tooltip("Used by spawns that have Random Position turned on.")]
    [SerializeField] Bounds randomSpawnArea = new Bounds(Vector3.zero, new Vector3(15.2f, 15f, 0f));
    [Tooltip("Which act's bomb positions to draw in the Scene view while this object is selected.")]
    [SerializeField] int previewActIndex;

    public int CurrentActIndex { get; private set; }
    public int CurrentEncounterIndex { get; private set; }
    public Act CurrentAct => encounterConfig != null ? encounterConfig.GetAct(CurrentActIndex) : null;

    public override string WaveLabel
    {
        get
        {
            Act act = CurrentAct;
            return act == null ? string.Empty : $"{act.actName}\n{CurrentEncounterIndex + 1}/{act.encounters.Count}";
        }
    }

    public override float WaveProgress
    {
        get
        {
            Act act = CurrentAct;
            return act == null || act.encounters.Count == 0 ? 0f : (float)CurrentEncounterIndex / act.encounters.Count;
        }
    }

    readonly List<Bomb> encounterBombs = new List<Bomb>();
    Coroutine actLoop;
    GameObject currentArena;

    public override void StartSpawning()
    {
        if (actLoop != null)
        {
            return;
        }

        if (encounterConfig == null)
        {
            Debug.LogError("ActSpawner: assign an EncounterConfiguration.", this);
            return;
        }

        actLoop = StartCoroutine(PlayActs());
    }

    public override void StopSpawning()
    {
        if (actLoop != null)
        {
            StopCoroutine(actLoop);
            actLoop = null;
        }
    }

    IEnumerator PlayActs()
    {
        yield return new WaitForSeconds(encounterConfig.initialDelay);

        int waveNumber = 0;
        for (CurrentActIndex = 0; ; CurrentActIndex++)
        {
            Act act = encounterConfig.GetAct(CurrentActIndex);
            if (act == null)
            {
                yield break;
            }

            BeginAct(act);
            for (CurrentEncounterIndex = 0; CurrentEncounterIndex < act.encounters.Count; CurrentEncounterIndex++)
            {
                GameEvents.RaiseWaveStarted(++waveNumber);
                yield return PlayEncounter(act, act.encounters[CurrentEncounterIndex]);
            }

            // Guards against a tight loop when every act is empty.
            yield return null;
        }
    }

    void BeginAct(Act act)
    {
        if (act.arenaGeometryPrefab != null)
        {
            if (currentArena != null)
            {
                Destroy(currentArena);
            }

            currentArena = Instantiate(act.arenaGeometryPrefab);
        }

        if (act.actStartEffectPrefab != null)
        {
            Instantiate(act.actStartEffectPrefab);
        }
    }

    IEnumerator PlayEncounter(Act act, Encounter encounter)
    {
        encounterBombs.Clear();

        foreach (BombSpawnData spawn in encounter.bombSpawns)
        {
            if (spawn.delayBeforeSpawn > 0f)
            {
                yield return new WaitForSeconds(spawn.delayBeforeSpawn);
            }

            Vector2 position = spawn.randomPosition ? UtilityClass.GetRandomPointInBounds(randomSpawnArea) : spawn.position;
            var modifiers = new BombModifiers(spawn.moving ? 1f : 0f, act.projectileSpeedMultiplier);
            Bomb bomb = spawnService.Spawn(spawn.bomb, position, modifiers);
            if (bomb != null)
            {
                encounterBombs.Add(bomb);
            }
        }

        if (encounter.waitForBombClear)
        {
            yield return new WaitUntil(() => encounterBombs.TrueForAll(bomb => bomb == null || bomb.IsResolved));
        }

        yield return new WaitForSeconds(encounter.delayBeforeNextEncounter);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, .5f, 0f, .6f);
        Gizmos.DrawWireCube(randomSpawnArea.center, randomSpawnArea.size);

        Act act = encounterConfig != null ? encounterConfig.GetAct(previewActIndex) : null;
        if (act == null)
        {
            return;
        }

        for (int i = 0; i < act.encounters.Count; i++)
        {
            Gizmos.color = Color.HSVToRGB((float)i / act.encounters.Count, .8f, 1f);
            foreach (BombSpawnData spawn in act.encounters[i].bombSpawns)
            {
                if (!spawn.randomPosition)
                {
                    Gizmos.DrawWireSphere(spawn.position, .5f);
                }
            }
        }
    }
}
