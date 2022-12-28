using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{


    

    [SerializeField]
    Missile missilePrefab;

    public bool spawning;

    public static MissileSpawner instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void StartSpawning()
    {

        if (spawning)
        {
            return;
        }

        spawning = true;
        StartCoroutine(StartSpawningMissiles());

        IEnumerator StartSpawningMissiles()
        {

            int missileCount = Random.Range(1, 5);
            for (int i = 0; i < missileCount; i++)
            {
                SpawnMissile();
            }

            yield return new WaitForSeconds(Random.Range(3f, 8f));

            StartCoroutine(StartSpawningMissiles());
        }

    }


    void SpawnMissile()
    {
        int randomSide = Random.Range(0, 4);
        Vector2 randomPos = new Vector2();
        Vector2 missileDirection= new Vector2();
        if (randomSide == 0)
        {
            randomPos.x = 11f;
            randomPos.y = Random.Range(-8, 9);
            missileDirection = Vector2.left;
        }
        else if (randomSide == 1)
        {
            randomPos.x = -11f;
            randomPos.y = Random.Range(-8, 9);
            missileDirection = Vector2.right;
        }
        else if (randomSide == 2)
        {
            randomPos.y = -11;
            randomPos.x = Random.Range(-8, 9);
            missileDirection = Vector2.up;
        }
        else if (randomSide == 3)
        {
            randomPos.y = 11f;
            randomPos.x = Random.Range(-8, 9);
            missileDirection = Vector2.down;
        }


        Missile m=   Instantiate(missilePrefab, randomPos, Quaternion.identity);
        m.SetDirection(missileDirection);
    }
}
