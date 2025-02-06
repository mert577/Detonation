using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCross : Bomb
{

    public GameObject DOTObject;

    public int length = 5;

    public int numberOfArms= 4;

    [SerializeField]
    float timeBetweenRings = .05f;

 
     [SerializeField] float resolutionIncreasePerRing = 0f;

    [SerializeField]
     bool lockedToPlayer = false;
     [SerializeField]
     float distanceBetweenRings = 1f;

    public override void Explode()
    {


        Transform player = GameObject.Find("Player").transform;
        Vector3 playerPos = player.position;
        base.Explode();
        StartCoroutine(DotSpawn());

        IEnumerator DotSpawn()
        {
           

            for(int i = 0; i < length; i++)
            {
                float realNumberOfArms = numberOfArms + i*resolutionIncreasePerRing;
                for (int arm =0; arm < Mathf.CeilToInt(realNumberOfArms); arm++ ){


                
                Vector2 baseDirection = new Vector2(1,0);

                if(lockedToPlayer){
                    baseDirection = (playerPos- transform.position).normalized;
                }

                Vector3 directionVector = Quaternion.Euler(0,0,(360f/Mathf.CeilToInt(realNumberOfArms))*arm) * baseDirection;
              
                GameObject a = Instantiate(DOTObject, transform.position+( directionVector*i*distanceBetweenRings), Quaternion.identity);

                }

           

                yield return new WaitForSeconds(timeBetweenRings);
            }
          
       
            Destroy(this.gameObject);
        }


    }


   
}
