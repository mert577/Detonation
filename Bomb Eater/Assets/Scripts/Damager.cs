using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Hurt(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }

    }

    public virtual void Hurt(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }

    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Hurt(collision);
    }


  

    private void OnCollisionStay2D(Collision2D collision)
    {
        Hurt(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hurt(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Hurt(collision);
    }

}
