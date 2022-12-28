using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDOT : Bomb
{
    public GameObject DOTObject;
    public override void Explode()
    {
        base.Explode();
        StartCoroutine(DotSpawn());

        IEnumerator DotSpawn()
        {
            yield return null;
            GameObject d = Instantiate(DOTObject, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


  
}
