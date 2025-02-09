using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTDamager : MonoBehaviour
{
    ParticleSystem p;

    [SerializeField]
    float lifetime = 2.5f;
    // Start is called before the first frame update

    [SerializeField]
    float originalScale = 1;

    void Awake()
    {
       originalScale = transform.localScale.x;
    }
    void Start()
    {
        p = GetComponent<ParticleSystem>();
        StartCoroutine(OnSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 300 * Time.deltaTime);
    }

    IEnumerator OnSpawn()
    {
        transform.localScale = Vector2.zero;
        transform.DOScale(Vector2.one * originalScale, 0.1f).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(0.1f);
        var em = p.emission;
        em.enabled = true;
        yield return new WaitForSeconds(lifetime);
        em.enabled = false;
        transform.DOScale(Vector2.zero, 0.2f).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(.2f);
        Destroy(this.gameObject);
    }
}
