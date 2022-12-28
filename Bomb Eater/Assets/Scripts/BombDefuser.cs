using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class BombDefuser : MonoBehaviour
{

    public bool ableToDefuse;


    public Transform closestBomb;
    public LayerMask layer;


    public GameObject target;

    public int defusedBombs = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       var Colliders=   Physics2D.OverlapCircleAll(transform.position, 2.5f,layer);
        if (Colliders.Length<=0)
        {
            closestBomb = null;
            target.SetActive(false);
        }
        else
        {
            target.SetActive(true);
            List<Collider2D> cols = Colliders.ToList();
            cols = cols.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();
            closestBomb = cols[0].transform;
            target.transform.position = closestBomb.position;

            Vector2 away = closestBomb.transform.position - transform.position;
            away.Normalize();
            target.transform.right = Quaternion.Euler(0, 0, 45)* away;
        }
       
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bomb bomb= collision.GetComponent<Bomb>();
        if (bomb != null)
        {
            if (ableToDefuse&& !bomb.setToExplode)
            {
                ProgressManager.instance.OnBombDefuse();
              
                GetComponentInParent<PlayerMovement>().dashCoolDownTimer = 0.1f;
                DOTween.To(() => Time.timeScale, SetTimeScale, .05f, 0.06f).OnComplete(() => Time.timeScale = 1f).SetUpdate(true);
                bomb.Defuse();
            }
            else
            {

                Vector2 away = bomb.transform.position - transform.position;
                away.Normalize();
                GetComponentInParent<PlayerMovement>().LoseControl(0.1f);
                GetComponentInParent<Rigidbody2D>().AddForce(away*-80, ForceMode2D.Impulse);
            }
            
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Bomb bomb = collision.GetComponent<Bomb>();
        if (bomb != null)
        {
            if (ableToDefuse && !bomb.setToExplode)
            {
                ProgressManager.instance.OnBombDefuse();
                DOTween.To(() => Time.timeScale, SetTimeScale, .05f, 0.06f).OnComplete(() => Time.timeScale = 1f).SetUpdate(true);
                GetComponentInParent<PlayerMovement>().dashCoolDownTimer = 0.1f;
                bomb.Defuse();

               
            }
            else
            {

                Vector2 away = bomb.transform.position - transform.position;
                away.Normalize();
                GetComponentInParent<PlayerMovement>().LoseControl(0.1f);
                GetComponentInParent<Rigidbody2D>().AddForce(away *- 80, ForceMode2D.Impulse);
            }

        }
    }


    void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }
}
