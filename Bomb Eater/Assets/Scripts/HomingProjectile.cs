using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{


   


    public float accelerationForce;


    [SerializeField]
     GameObject player;


    public override void StartProjectile(float angle, float speedMultip = 1)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector2 dir =  UtilityClass.GetDirectionFromRotation(angle);


        dir.Normalize();
        rb.linearVelocity = dir * speed*speedModifier;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
   

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 dir = transform.position - player.transform.position;
        dir.Normalize();

        rb.AddForce(accelerationForce * -dir);  //*  Random.Range(.5f,1.5f));
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, speed* speedModifier);


        transform.rotation = Quaternion.Euler(0, 0, UtilityClass.GetRotationFromDirection(rb.linearVelocity.normalized)); 
    }



}
