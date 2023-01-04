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
        rb.velocity = dir * speed*speedModifier;
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
    void Update()
    {
        Vector2 dir = transform.position - player.transform.position;
        dir.Normalize();

        rb.AddForce(accelerationForce * -dir);  //*  Random.Range(.5f,1.5f));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed* speedModifier);


        transform.rotation = Quaternion.Euler(0, 0, UtilityClass.GetRotationFromDirection(rb.velocity.normalized)); 
    }



}
