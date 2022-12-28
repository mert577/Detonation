using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCross : Bomb
{

    public GameObject DOTObject;

    public override void Explode()
    {
        base.Explode();
        StartCoroutine(DotSpawn());

        IEnumerator DotSpawn()
        {
           

            for(int i = 0; i < 5; i++)
            {
                GameObject a = Instantiate(DOTObject, transform.position+( Vector3.right*i), Quaternion.identity);
                GameObject b = Instantiate(DOTObject, transform.position + (Vector3.left * i), Quaternion.identity);
                GameObject c = Instantiate(DOTObject, transform.position + (Vector3.down * i), Quaternion.identity);

                GameObject d = Instantiate(DOTObject, transform.position + (Vector3.up * i), Quaternion.identity);

                yield return new WaitForSeconds(.05f);
            }
          
            Destroy(this.gameObject);
        }
    }
}
