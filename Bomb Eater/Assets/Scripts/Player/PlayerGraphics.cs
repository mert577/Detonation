using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// Visual and audio feedback for the player's health changes.
/// </summary>
public class PlayerGraphics : MonoBehaviour
{
    public GameObject graphicsObject;

    [Header("Heal")]
    [SerializeField] GameObject healthAnimation;
    [SerializeField] float moveAmount;
    [SerializeField] float animationTime;
    [SerializeField] AudioClip healSound;
    [SerializeField] AudioSource audioSource;

    [Header("Hurt")]
    [SerializeField] float hurtPunchScale = .8f;
    [SerializeField] float hurtPunchTime = .3f;

    [Header("Parts used by PlayerDashVisuals")]
    public SpriteRenderer playerCoreSpriteRenderer;
    public ParticleSystem playerShellParticles;
    public ParticleSystem playerTrailParticles;

    public void PlayHurtAnimation()
    {
        graphicsObject.transform.DORewind();
        graphicsObject.transform.DOPunchScale(Vector3.one * hurtPunchScale, hurtPunchTime);
    }

    public void PlayHealthAnimation()
    {
        Transform popup = healthAnimation.transform;
        TextMeshPro popupText = healthAnimation.GetComponent<TextMeshPro>();

        popup.DOKill();
        popupText.DOKill();
        healthAnimation.SetActive(true);
        popup.position = transform.position;
        popupText.alpha = 1f;

        popup.DOMoveY(transform.position.y + moveAmount, animationTime).SetEase(Ease.OutQuint).OnComplete(() => healthAnimation.SetActive(false));
        popupText.DOFade(0f, animationTime);

        if (audioSource != null && healSound != null)
        {
            audioSource.PlayOneShot(healSound);
        }
    }

    public void PlayDeath()
    {
        graphicsObject.SetActive(false);
    }
}
