using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
   
    public static ProgressManager instance;

    public int waveNumber;

    public int bombsDefused;

    

    public Difficulty currentDifficulty;


    public float movementChance;

    public float waveSpawnTimeModifier;

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
        currentDifficulty = CalculateDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
         currentDifficulty= CalculateDifficulty();
    }


    
    public void OnBombDefuse()
    {
        bombsDefused += 1;
        if (bombsDefused % 35 == 0)
        {
            PlayerHealth playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
            playerHealth.Heal();
        }
    }

    Difficulty CalculateDifficulty()
    {
        Difficulty dif = Difficulty.Easy;

        if (waveNumber < 15)
        {
            dif = Difficulty.Easy;
            movementChance = 0;
            Projectile.speedModifier = 1f;
            waveSpawnTimeModifier = 1;
        }
        else if(waveNumber>=15& waveNumber < 25)
        {
            dif = Difficulty.Medium;
            movementChance = .35f;
            Projectile.speedModifier = 1.1f;
            waveSpawnTimeModifier = 1f;
        }
        else if (waveNumber >= 25 && waveNumber < 40)
        {
            dif = Difficulty.Hard;
            movementChance = .65f;
            Projectile.speedModifier = 1.25f;
            waveSpawnTimeModifier = .75f;
        }
        else if (waveNumber >= 40)
        {
            dif = Difficulty.Extreme;
            movementChance = .85f;
            Projectile.speedModifier = 1.4f;
            waveSpawnTimeModifier = .6f;
        }

        if(dif == Difficulty.Hard)
        {
            MissileSpawner.instance.StartSpawning();
        }

        return dif;



    }
}


public enum Difficulty
{
    Easy=0,
    Medium=1,
    Hard=2,
    Extreme=3
}
