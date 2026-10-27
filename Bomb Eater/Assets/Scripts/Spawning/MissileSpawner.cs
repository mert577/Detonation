using System.Collections;
using UnityEngine;

/// <summary>
/// Sends volleys of missiles in from a random side of the arena while spawning is on.
/// </summary>
public class MissileSpawner : MonoBehaviour
{
    static readonly Vector2[] FlightDirections = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

    [SerializeField] Missile missilePrefab;
    [Tooltip("Distance from the centre where missiles appear.")]
    [SerializeField] float spawnDistance = 11f;
    [Tooltip("Missiles appear at a whole-number offset in this +/- range along their side.")]
    [SerializeField] int lateralRange = 8;
    [Tooltip("Inclusive min and max missiles per volley.")]
    [SerializeField] Vector2Int missilesPerVolley = new Vector2Int(1, 4);
    [SerializeField] Vector2 timeBetweenVolleys = new Vector2(3f, 8f);

    public float SpeedMultiplier { get; set; } = 1f;
    public bool IsSpawning => spawnLoop != null;

    Coroutine spawnLoop;

    public void StartSpawning()
    {
        if (spawnLoop == null)
        {
            spawnLoop = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        if (spawnLoop != null)
        {
            StopCoroutine(spawnLoop);
            spawnLoop = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            int count = Random.Range(missilesPerVolley.x, missilesPerVolley.y + 1);
            for (int i = 0; i < count; i++)
            {
                SpawnMissile();
            }

            yield return new WaitForSeconds(Random.Range(timeBetweenVolleys.x, timeBetweenVolleys.y));
        }
    }

    void SpawnMissile()
    {
        Vector2 direction = FlightDirections[Random.Range(0, FlightDirections.Length)];
        int lateral = Random.Range(-lateralRange, lateralRange + 1);
        Vector2 position = -direction * spawnDistance + Vector2.Perpendicular(direction) * lateral;

        Missile missile = Instantiate(missilePrefab, position, Quaternion.identity);
        missile.Init(direction, SpeedMultiplier);
    }
}
