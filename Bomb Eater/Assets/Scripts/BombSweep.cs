using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSweep : Bomb
{


    public GameObject bullet;

    public int bulletCount;

    public int burstCount;

    public float burstInterval;

    public float angleOffset;
    // Start is called before the first frame update

    public override void Explode()
    {

        base.Explode();
        StartCoroutine(explosion());





    }


    IEnumerator explosion()
    {
        

            for (int i = 0; i < bulletCount; i++)
            {
                float angle =  (i * (180 / bulletCount))+90f;
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.GetComponent<Projectile>().StartProjectile(angle);

               angle =  180f+  (i * (180 / bulletCount))+90f;
               GameObject a = Instantiate(bullet, transform.position, Quaternion.identity);
               a.GetComponent<Projectile>().StartProjectile(angle);
               yield return new WaitForSeconds(burstInterval);
            }


           
        
        Destroy(gameObject);
    }
}
