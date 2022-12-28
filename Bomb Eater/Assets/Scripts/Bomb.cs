using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{


    public float explodeTime;
    public float timeLeft;
    public float explosionRadius;
    public GameObject explodeParticle;
    public GameObject defuseParticle;

    GameObject graphics;

    Transform shadow;

    public bool setToExplode;



    float blinkTime= 1f;


    bool isCountingDown;
    bool blinking;


    private void Start()
    {
        timeLeft = explodeTime;
        graphics = transform.GetChild(0).gameObject;
        shadow = transform.GetChild(1);
        StartCoroutine(StartAnim());
    }

    private void Update()
    {
        if (isCountingDown)
        {
            timeLeft -= Time.deltaTime;
        }


        shadow.localScale = Vector3.one *     ((7 - graphics.transform.localPosition.y)/7);
        

        if (timeLeft <= 0)
        {
            if (!setToExplode)
            {
                
                Explode();
            }
       
        }

        //Start blinking when one second left to explosion
        else if (timeLeft <= blinkTime)
        {

            if (!blinking)
            {
                
                Blink();
            }
        }
    }

    
    IEnumerator StartAnim()
    {
        graphics.transform.localPosition = Vector2.up * 7f;
        graphics.transform.DOLocalMoveY(0, 1f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(1f);
        isCountingDown = true;
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
        if(TryGetComponent(out BombMovement movement))
        {
            if(Random.value< ProgressManager.instance.movementChance)
            {
                movement.StartMoving();
            }
          
        }
    }

    void Blink()
    {
        blinking = true;
        Color c = Color.white;
        graphics.transform.DOScale(graphics.transform.localScale * 1.2f, blinkTime).SetEase(Ease.OutQuad);
        graphics.transform.DOShakePosition(blinkTime, .15f, 15);
        graphics.GetComponent<SpriteRenderer>().DOColor(Color.white, blinkTime).SetEase(Ease.OutQuad);
    }


    public virtual void Explode()
    {
        setToExplode = true;
        Camera.main.DOShakePosition(.25f, .3f).OnComplete(() => Camera.main.transform.position = new Vector3(0, 0, -10));
        
        GetComponent<BombMovement>().StopMoving();
        GameObject p = Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Destroy(p, 1f);
       
    }

    public virtual void Defuse()
    {
        Camera.main.DOShakePosition(.1f, .5f).OnComplete(() => Camera.main.transform.position=new Vector3(0,0,-10));
        GameObject p = Instantiate(defuseParticle, transform.position, Quaternion.identity);
        Destroy(p, 1f);
        Destroy(gameObject);
        
    }
}
