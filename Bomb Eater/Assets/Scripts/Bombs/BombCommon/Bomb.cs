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
    [SerializeField] Transform playerTransform;

    [SerializeField] GameObject graphics;

    Transform shadow;

    public bool setToExplode;

    float blinkTime= 1f;

    bool isCountingDown;
    bool blinking;

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    protected Transform GetPlayerTransform()
    {
        if (playerTransform == null && BombSpawner.instance != null)
        {
            playerTransform = BombSpawner.instance.PlayerTransform;
        }

        return playerTransform;
    }

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
       // graphics.transform.DOScale(graphics.transform.localScale * 1.2f, blinkTime).SetEase(Ease.OutQuad);
        graphics.transform.DOShakePosition(blinkTime, .25f, 15);
        graphics.GetComponent<SpriteRenderer>().DOColor(Color.white, blinkTime).SetEase(Ease.OutQuad);
    }

    public virtual void Explode()
    {
        setToExplode = true;
        BombEffects.PlayCameraShake(.25f, .3f);
        graphics.SetActive(false);
        BombEffects.StopRigidBody(GetComponent<Rigidbody2D>());
        BombEffects.SpawnParticle(explodeParticle, transform.position, 1f);

    }

    public virtual void Defuse()
    {
        BombEffects.PlayCameraShake(.1f, .5f);
        BombEffects.SpawnParticle(defuseParticle, transform.position, 1f);
        Destroy(gameObject);

    }
}

public static class BombEffects
{
    static readonly Vector3 CameraResetPosition = new Vector3(0f, 0f, -10f);

    public static void PlayCameraShake(float duration, float strength)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        mainCamera.DOShakePosition(duration, strength).OnComplete(() =>
        {
            if (mainCamera != null)
            {
                mainCamera.transform.position = CameraResetPosition;
            }
        });
    }

    public static void StopRigidBody(Rigidbody2D body)
    {
        if (body == null)
        {
            return;
        }

        body.bodyType = RigidbodyType2D.Kinematic;
        body.linearVelocity = Vector2.zero;
    }

    public static void SpawnParticle(GameObject particlePrefab, Vector3 position, float destroyDelay)
    {
        if (particlePrefab == null)
        {
            return;
        }

        GameObject particle = Object.Instantiate(particlePrefab, position, Quaternion.identity);
        Object.Destroy(particle, destroyDelay);
    }
}
