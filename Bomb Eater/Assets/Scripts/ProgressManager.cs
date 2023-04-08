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

        if (waveNumber < 9)
        {
            dif = Difficulty.Easy;
            movementChance = 0;
            Projectile.speedModifier = 1f;
        }
        else if(waveNumber>=9& waveNumber < 18)
        {
            dif = Difficulty.Medium;
            movementChance = .35f;
            Projectile.speedModifier = 1.1f;
        }
        else if (waveNumber >= 18 && waveNumber < 25)
        {
            dif = Difficulty.Hard;
            movementChance = .65f;
            Projectile.speedModifier = 1.25f;
        }
        else if (waveNumber >= 25)
        {
            dif = Difficulty.Extreme;
            movementChance = .85f;
            Projectile.speedModifier = 1.4f;
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
