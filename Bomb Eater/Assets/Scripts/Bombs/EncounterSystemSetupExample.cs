using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example scene setup for the encounter-based bomb spawning system
/// This shows how to configure acts, encounters, and link them to ActSpawner
/// 
/// SETUP STEPS:
/// 1. Create a new ScriptableObject of type "EncounterConfiguration" (Right-click > Create > Game > Encounter Configuration)
/// 2. Assign bomb prefabs to encounters
/// 3. Position bombs using the Inspector or EncounterBuilder
/// 4. Assign the EncounterConfiguration to ActSpawner
/// 5. Call ActSpawner.instance.StartSpawning() when ready to begin
/// </summary>
public class EncounterSystemSetupExample : MonoBehaviour
{
    [SerializeField] private EncounterConfiguration encounterConfig;
    [SerializeField] private ActSpawner actSpawner;

    private void Start()
    {
        // Example of starting the system
        if (actSpawner != null && encounterConfig != null)
        {
            actSpawner.StartSpawning();
        }
    }

    /// <summary>
    /// Example of programmatically creating acts and encounters
    /// Useful for procedural or scripted encounter generation
    /// </summary>
    public void SetupEncountersFromCode()
    {
        // Get bomb prefabs (from pools or direct references)
        Bomb easyBomb = GetComponent<Bomb>(); // Replace with actual reference

        if (easyBomb == null)
        {
            Debug.LogError("No bomb prefab found!");
            return;
        }

        // Create Act 1 with arena geometry
        Act act1 = EncounterBuilder.CreateAct("Act 1: Starting Area", null);

        // Create encounters for Act 1
        Encounter enc1 = EncounterBuilder.CreateCircleEncounter(
            "Circular Pattern",
            easyBomb,
            Vector3.zero,
            radius: 5f,
            bombCount: 8,
            staggerDelay: 0.1f
        );
        enc1.delayBeforeNextEncounter = 1f;

        Encounter enc2 = EncounterBuilder.CreateGridEncounter(
            "Grid Pattern",
            easyBomb,
            Vector3.zero,
            rows: 3,
            cols: 3,
            spacing: 2f,
            staggerDelay: 0.05f
        );
        enc2.delayBeforeNextEncounter = 1.5f;

        EncounterBuilder.AddEncounter(act1, enc1);
        EncounterBuilder.AddEncounter(act1, enc2);

        // Create Act 2
        Act act2 = EncounterBuilder.CreateAct("Act 2: Challenge Zone", null);

        Encounter enc3 = EncounterBuilder.CreateEncounter("Random Chaos");
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-8f, 8f),
                0,
                Random.Range(-8f, 8f)
            );
            EncounterBuilder.AddBombSpawn(enc3, easyBomb, randomPos, Random.Range(0f, 0.5f));
        }
        enc3.delayBeforeNextEncounter = 2f;

        EncounterBuilder.AddEncounter(act2, enc3);

        // Add acts to config
        encounterConfig.acts.Clear();
        encounterConfig.acts.Add(act1);
        encounterConfig.acts.Add(act2);

        Debug.Log("Encounters created from code!");
    }

    /// <summary>
    /// Example of listening to encounter events
    /// </summary>
    private void OnEnable()
    {
        ActSpawner.OnActStart += HandleActStart;
        ActSpawner.OnActEnd += HandleActEnd;
        ActSpawner.OnEncounterStart += HandleEncounterStart;
        ActSpawner.OnEncounterEnd += HandleEncounterEnd;
    }

    private void OnDisable()
    {
        ActSpawner.OnActStart -= HandleActStart;
        ActSpawner.OnActEnd -= HandleActEnd;
        ActSpawner.OnEncounterStart -= HandleEncounterStart;
        ActSpawner.OnEncounterEnd -= HandleEncounterEnd;
    }

    private void HandleActStart()
    {
        Debug.Log($"Act started: {ActSpawner.instance.CurrentAct.actName}");
        // Trigger UI updates, camera changes, music, etc.
    }

    private void HandleActEnd()
    {
        Debug.Log($"Act ended: {ActSpawner.instance.CurrentAct.actName}");
    }

    private void HandleEncounterStart()
    {
        Debug.Log($"Encounter started: {ActSpawner.instance.CurrentEncounter.encounterName}");
    }

    private void HandleEncounterEnd()
    {
        Debug.Log($"Encounter ended: {ActSpawner.instance.CurrentEncounter.encounterName}");
    }
}
