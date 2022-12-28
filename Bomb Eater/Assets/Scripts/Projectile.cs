using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    [SerializeField] ParticleSystem particles;
    protected  Rigidbody2D rb;

    public static float speedModifier = 1;


    public virtual void StartProjectile(float angle, float speedMultip=1)
    {
      
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = transform.right * speed * speedModifier*speedMultip;
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
        transform.rotation = Quaternion.Euler(0, 0, UtilityClass.GetRotationFromDirection(rb.velocity.normalized));
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        ParticleSystem p = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(p, 1f);
        StartCoroutine(DestoryItself());

        IEnumerator DestoryItself()
        {
            yield return new WaitForEndOfFrame();
            Destroy(this.gameObject);
        }
    }



}
