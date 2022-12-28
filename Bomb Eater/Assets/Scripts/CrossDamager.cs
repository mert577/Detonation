using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossDamager : MonoBehaviour
{

    LineRenderer lr;

    public AnimationCurve ac;

    public float activeTime;

    public bool vertical;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }


    IEnumerator Activate()
    {

        yield return new WaitForSeconds(.1f);
        lr.enabled = true;
        float baseWidth = lr.startWidth;


        float timer = 0;
        while (timer < activeTime)
        {
            timer += Time.deltaTime;

            float t = timer / activeTime;
            float width = ac.Evaluate(t)*baseWidth;

            lr.endWidth = width;
            lr.startWidth = width;

            if(t>.1f)
                {
                if (vertical)
                {
                    lr.SetPosition(0, new Vector2(transform.position.x, transform.position.y + 4f));
                    lr.SetPosition(1, new Vector2(transform.position.x, transform.position.y - 4f));
                }
                else
                {
                    lr.SetPosition(0, new Vector2(transform.position.x + 4f, transform.position.y));
                    lr.SetPosition(1, new Vector2(transform.position.x - 4f, transform.position.y));
                }
            }
            else
            {
                if (vertical)
                {
                    lr.SetPosition(0, new Vector2(transform.position.x, transform.position.y +  (t/.1f)  *4f));
                    lr.SetPosition(1, new Vector2(transform.position.x, transform.position.y -   (t/.1f) *4f));
                }
                else
                {
                    lr.SetPosition(0, new Vector2(transform.position.x + (t / .1f) * 4f, transform.position.y));
                    lr.SetPosition(1, new Vector2(transform.position.x - (t / .1f) * 4f, transform.position.y));
                }
            }
            


            if (t > .9f)
            {
                GetComponent<Collider2D>().enabled = false;
            }
            else if (t < .12f)
            {
                GetComponent<Collider2D>().enabled = true;
            }

            yield return null;


        }
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
       
       
        StartCoroutine(Activate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
