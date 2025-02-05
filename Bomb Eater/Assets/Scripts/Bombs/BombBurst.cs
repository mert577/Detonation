using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBurst : Bomb
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
        for(int j = 0; j < burstCount; j++)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = angleOffset + (i * (360f / bulletCount))   + ((120f / bulletCount))*j;
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.GetComponent<Projectile>().StartProjectile(angle);
            }


            yield return new WaitForSeconds(burstInterval);
        }
        Destroy(gameObject);
    }
}
