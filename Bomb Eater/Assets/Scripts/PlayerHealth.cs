using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{

    public int MaxHealth;
    public int CurrentHealth;

    public GameObject damageParticles;
    public GameObject deathParticles;



    public float invincibilityTime;
    public float invincibilityTimer;
    public bool invincible;

    public bool gotDamagedAlready;

    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInvincibility();
    }



    public void Heal()
    {
      
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += 1;
        }
    }

    private void LateUpdate()
    {
        gotDamagedAlready = false;
    }

    public void TakeDamage()
    {
        if (invincible || isDead || gotDamagedAlready)
        {
            return;
        }

        StartCoroutine(_());

        IEnumerator _()
        {
            

            CurrentHealth -= 1;
            gotDamagedAlready = true;

            SetTimeScale(0f);
            yield return new WaitForSecondsRealtime(.1f);
            SetTimeScale(1f);

            transform.GetChild(0).DOPunchScale(Vector3.one * 3, 0.3f).OnComplete(() => transform.GetChild(0).localScale = Vector3.one);
            GetComponent<PlayerMovement>().LoseControl(0.2f);
            invincibilityTimer = invincibilityTime;
            invincible = invincibilityTimer > 0;
            Camera.main.DOShakePosition(0.3f, 1f, 13);

            GameObject p = Instantiate(damageParticles, transform.position, Quaternion.identity);
            Destroy(p, 1);
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                if(!isDead){
                    StartCoroutine(Death());
                }
                   
            }


        }



    }

    void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    void HandleInvincibility()
    {
        invincible = invincibilityTimer > 0;
        invincibilityTimer -= Time.deltaTime;
        invincibilityTimer = Mathf.Clamp(invincibilityTimer, 0, 23123);

       

    }

    IEnumerator Death() {

       

        isDead = true;

    
        GameObject p = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(p, 5);


        GetComponent<PlayerGraphics>().graphicsObject.SetActive(false);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<BombDefuser>().enabled = false;

        yield return new WaitForSeconds(2f);

        SceneLoadManager.instance.OnReloadScene();
        //call death event
    }
}
