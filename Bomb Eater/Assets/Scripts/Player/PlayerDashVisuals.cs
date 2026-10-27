using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PlayerDash))]
public class PlayerDashVisuals : MonoBehaviour
{
    public int DashAfterImageCount = 3;

    [Header("Dash visuals")]
    [SerializeField] float rotateSpeed = 360f;
    [SerializeField] PlayerGraphics playerGraphics;
    [SerializeField] Color dashColor = Color.cyan;
    [SerializeField] GameObject ableToDashParticles;
    [SerializeField] GameObject afterImage;
    [SerializeField] ParticleSystem trailParticles;

    Transform playerGraphicsTransform;
    SpriteRenderer playerSpriteRenderer;
    Color originalColor;
    Color originalColorParticles;
    Vector3 originalGraphicsScale;
    PlayerDash playerDash;

    void Awake()
    {
        playerDash = GetComponent<PlayerDash>();

        if (playerGraphics != null)
        {
            playerGraphicsTransform = playerGraphics.transform;
            playerSpriteRenderer = playerGraphics.playerCoreSpriteRenderer;
            originalGraphicsScale = playerGraphicsTransform.localScale;
            originalColor = playerSpriteRenderer != null ? playerSpriteRenderer.color : Color.white;
            originalColorParticles = playerGraphics.playerShellParticles != null ? playerGraphics.playerShellParticles.main.startColor.color : Color.white;
       
        }
    }

    void Update()
    {
        RotateGraphics();
        UpdateCooldownTint();
    }

    void RotateGraphics()
    {
        if (playerGraphicsTransform == null)
        {
            return;
        }

        playerGraphicsTransform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
    }

    void UpdateCooldownTint()
    {
        if (playerSpriteRenderer == null || playerDash == null)
        {
            return;
        }

        bool onCooldown = playerDash.CooldownTimer > 0f;
        playerSpriteRenderer.color = (onCooldown && !playerDash.IsDashing) ? dashColor : originalColor;
    
        Color particleColor = (onCooldown && !playerDash.IsDashing) ? dashColor : originalColorParticles;
        //change the particle colors as well
        if (trailParticles != null)
        {
            var main = trailParticles.main;
            main.startColor = particleColor;
        }
        if (playerGraphics.playerShellParticles != null)
        {
            var main = playerGraphics.playerShellParticles.main;
            main.startColor =  particleColor;
        }
    
    }

    public void PlayDashStart(float dashTime)
    {
        if (playerGraphicsTransform != null)
        {
            playerGraphicsTransform.DORewind();
            playerGraphicsTransform.DOPunchScale(originalGraphicsScale * 1.35f, dashTime);
        }

        if (trailParticles != null)
        {
            var emission = trailParticles.emission;
            emission.enabled = false;
        }

        StartCoroutine(SpawnAfterImages(dashTime, DashAfterImageCount));
    }

    public void PlayDashEnd()
    {
        if (trailParticles != null)
        {
            var emission = trailParticles.emission;
            emission.enabled = true;
        }
    }

    public void PlayDashReady()
    {
        if (ableToDashParticles == null)
        {
            return;
        }

        GameObject particles = Instantiate(ableToDashParticles, transform);
        Destroy(particles, 1f);
    }

    IEnumerator SpawnAfterImages(float dashTime, int count)
    {
        if (afterImage == null)
        {
            yield break;
        }

        for (int i = 0; i < count; i++)
        {
            Instantiate(afterImage, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(dashTime / count);
        }
    }
}
