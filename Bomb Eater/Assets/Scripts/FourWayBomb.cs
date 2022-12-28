using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourWayBomb : Bomb
{

    
    public GameObject bullet;

    public int bulletCount;

    public float angleOffset;
    // Start is called before the first frame update

    public override void Explode()
    {

        base.Explode();
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleOffset + (i * (360f/bulletCount));
            GameObject b =   Instantiate(bullet, transform.position, Quaternion.identity);
            b.GetComponent<Projectile>().StartProjectile(angle);
        }




        Destroy(gameObject);
    }
}
