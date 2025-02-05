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

    public override void Explode()
    {
        base.Explode();
        StartCoroutine(DotSpawn());

        IEnumerator DotSpawn()
        {
           

            for(int i = 0; i < length; i++)
            {
                float realNumberOfArms = numberOfArms + i*resolutionIncreasePerRing;
                for (int arm =0; arm < Mathf.CeilToInt(realNumberOfArms); arm++ ){
                Vector3 directionVector = Quaternion.Euler(0,0,(360f/Mathf.CeilToInt(realNumberOfArms))*arm) * new Vector2(1, 0);
                GameObject a = Instantiate(DOTObject, transform.position+( directionVector*i), Quaternion.identity);

                }

           

                yield return new WaitForSeconds(timeBetweenRings);
            }
          
       
            Destroy(this.gameObject);
        }


    }


   
}
