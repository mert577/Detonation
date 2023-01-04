using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTarget : Bomb
{
    public GameObject bullet;
    [SerializeField] Transform target;
    [SerializeField] float angleOffset;
    public int bulletCount;
    public float bulletSpread;

    public float spreadRandomness;
    public float speedRandomness;

    public override void Explode()
    {
        target = GameObject.Find("Player").transform;
        base.Explode();
        for (int i = -bulletCount/2; i < (bulletCount/2)+1; i++)
        {

            Vector2 dir =target.position - transform.position;
            dir.Normalize();

            angleOffset = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle = angleOffset + (i * (bulletSpread* (1+Random.Range(-spreadRandomness, spreadRandomness))));
            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.GetComponent<Projectile>().StartProjectile(angle, 1f+ ( Random.Range (-speedRandomness,speedRandomness)));
         
        }




        Destroy(gameObject);
    }

}
