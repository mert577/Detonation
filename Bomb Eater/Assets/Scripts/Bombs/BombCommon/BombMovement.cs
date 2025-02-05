using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{


    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] bool isMoving;


    [SerializeField] Vector2 vel;


    
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void StartMoving()
    {
        isMoving = true;

        Vector2 dir = Random.insideUnitCircle;
        dir.Normalize();
        rb.velocity =dir * speed* Random.Range(.7f,1f);
        vel = rb.velocity;

    }

    public void StopMoving()
    {

        if (!isMoving)
        {
            return;
        }
        isMoving = false;


        rb.velocity = Vector2.zero;
       

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        
      vel =Vector2.Reflect(vel, collision.contacts[0].normal);
        rb.velocity = vel;
    }
}
