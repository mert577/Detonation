using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Missile : MonoBehaviour
{


    SpriteRenderer sr;
    [SerializeField] Vector2 direction;
    [SerializeField] Projectile missile;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SendMissile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    IEnumerator SendMissile()
    {
        sr.DOFade(0, 1.5f).SetEase(Ease.Flash, 8);
        yield return new WaitForSeconds(1.5f);
        Projectile b = Instantiate(missile, transform.position, Quaternion.identity) as Projectile;
        b.StartProjectile(UtilityClass.GetRotationFromDirection(direction));
        b.transform.right = direction;
        Destroy(this.gameObject);

    }
}

