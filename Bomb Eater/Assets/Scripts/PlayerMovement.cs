using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;

    public Vector2 input;

    public float maxSpeed;

    public float accelerationForce;


    public bool dashing;

    public float dashForce;

    public float dashDrag;
    public float dashTime;


    //public ParticleSystem DashParticles;

    public float rotateSpeed;

    public GameObject playerGraphics;


    public float dashCooldown;


    public float dashCoolDownTimer;
    bool control= true;
    public Color dashColor;


    public int maxDashes;

    public int dashesLeft;

    public GameObject ableToDashParticles;

    public GameObject afterImage;
    Color originalColor;

    BombDefuser defuser;

    Vector2 originalSize;
    public ParticleSystem trailParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSize = transform.localScale;
        defuser = transform.GetChild(1).GetComponent<BombDefuser>();
    }
    // Start is called before the first frame update
    void Start()
    {
        dashesLeft = maxDashes;
        originalColor = playerGraphics.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);


        playerGraphics.transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);

        if (!dashing)
        {
           
            if (Input.GetKeyDown(KeyCode.Space) && dashCoolDownTimer  <= 0)
            {
                StartCoroutine(Dash());
            }
        }

        HandleDashCooldown();



        if (!(dashCoolDownTimer <= 0) && !dashing)
        {
            
            playerGraphics.GetComponent<SpriteRenderer>().color = dashColor;
        }
        else
        {
            playerGraphics.GetComponent<SpriteRenderer>().color = originalColor;
        }
        
    }


    private void FixedUpdate()
    {
        if (!dashing)
        {



            if (!control)
            {
                input = Vector2.zero;
            }
            rb.AddForce(accelerationForce * input);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
           
        }

    }

    void HandleDashCooldown()
    {

        float oldTime = dashCoolDownTimer;
        dashCoolDownTimer -= Time.deltaTime;
        dashCoolDownTimer = Mathf.Clamp(dashCoolDownTimer, 0f, Mathf.Infinity);

        if(oldTime>0 && dashCoolDownTimer <= 0)
        {
            GameObject p = Instantiate(ableToDashParticles, transform);
            dashesLeft = maxDashes;
            Destroy(p, 1f);

        }

    }

    IEnumerator Dash()
    {

        GetComponent<PlayerHealth>().invincibilityTimer = dashTime+0.1f;
        dashing = true;

        dashesLeft--;

        if (dashesLeft <= 0)
        {
            dashesLeft = 0;
            dashCoolDownTimer = dashCooldown;
        }

       

        playerGraphics.transform.DOPunchScale(originalSize * 1.35f,dashTime);

        defuser.ableToDefuse = true;
        float drag = rb.drag;
        Vector2 direction;
        if (defuser.closestBomb != null)
        {
            
            direction = defuser.closestBomb.position - transform.position;
            direction.Normalize();
        }
        else
        {
            direction = input;
        }
        
        rb.velocity =  dashForce * direction;
        Debug.Log(rb.velocity);
         var em = trailParticles.emission;
         em.enabled =false;
        rb.drag = dashDrag;
        float timer =dashTime;
        StartCoroutine(SpawnAfterImages(3));
        while (timer > 0)
        {
            float t = timer / dashTime;
           

          


            

            timer -= Time.deltaTime;
            rb.drag -= (dashDrag - drag) / (dashTime / Time.deltaTime);
            yield return null;
        }
        em.enabled = true;
        //rb.velocity = Vector2.zero;
        rb.drag = drag;
        dashing = false;
        transform.localScale = (originalSize);
        defuser.ableToDefuse = false;

    }



    IEnumerator SpawnAfterImages(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Instantiate(afterImage, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(dashTime / count);
        }
       
    }
    public void LoseControl(float time)
    {
        StartCoroutine(_LoseControl(time));
    }


    IEnumerator _LoseControl(float time)
    {
        control = false;
        yield return new WaitForSeconds(time);
        control = true;

    }
}
