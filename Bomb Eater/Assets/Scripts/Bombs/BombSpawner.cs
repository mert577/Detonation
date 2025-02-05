using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{


    public float waveWaitTime;

    public int minWaveSize;
    public int maxWaveSize;


    public List<Bomb> easyBombPool;
    public List<Bomb> mediumBombPool;
    public List<Bomb> hardBombPool;
    public List<Bomb> extremeBombPool;



    public Bounds bounds;

    public List<Bomb> bombsToSpawn;

    public static BombSpawner instance;

    public float waitTime;

    public float timer;


    public bool spawning;
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
        bombsToSpawn = easyBombPool;

     //   bounds.center = transform.position;
      //  bounds.extents = transform.localScale- Vector3.one*.4f;
       
    
    
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
         StartCoroutine(SpawnBombWave());
    }

    void CalculateBombPool()
    {
        Difficulty dif = ProgressManager.instance.currentDifficulty;
        if (dif == Difficulty.Easy)
        {
            bombsToSpawn = easyBombPool;
        }
        else if (dif == Difficulty.Medium)
        {
            bombsToSpawn = mediumBombPool;
        }
        else if (dif == Difficulty.Hard)
        {
            bombsToSpawn = hardBombPool;
        }
        else if (dif == Difficulty.Extreme)
        {
            bombsToSpawn = extremeBombPool;
        }
    }

    IEnumerator SpawnBombWave()
    {

        int waveNumber = ProgressManager.instance.waveNumber;

        int waveNumberMod = waveNumber % 7;
        
        float waveNumberSpawnTimeModifier = 1;
        if(waveNumberMod >= 5){
            waveNumberSpawnTimeModifier =.75f;
        }

        waitTime = waveWaitTime* ProgressManager.instance.waveSpawnTimeModifier* waveNumberSpawnTimeModifier + Random.Range(-.2f, .2f);
        timer = waitTime;
        ProgressManager.instance.waveNumber += 1;
        CalculateBombPool();
        for (int i = 0; i < Random.Range(minWaveSize, maxWaveSize + 1); i++)
        {
            Bomb randomBomb = bombsToSpawn[Random.Range(0, bombsToSpawn.Count)];
            Vector2 randomPos = UtilityClass.GetRandomPointInBounds(bounds);
            Instantiate(randomBomb, randomPos, Quaternion.identity);


            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }


        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return  null;
        }

        timer = 0;

        StartCoroutine(SpawnBombWave());
    }
}
