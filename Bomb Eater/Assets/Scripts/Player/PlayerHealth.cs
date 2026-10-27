using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Hearts, invincibility frames, hurt feedback and death. Raises GameEvents for the rest of the game;
/// the UnityEvents are free hooks for extra feedback (sounds, effects) set up in the Inspector.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [FormerlySerializedAs("MaxHealth")]
    [SerializeField] int maxHealth = 5;
    [Tooltip("Seconds of invincibility after taking damage.")]
    [SerializeField] float invincibilityTime;

    [Header("Hurt feedback")]
    [SerializeField] GameObject damageParticles;
    [SerializeField] float hurtTimeScale = .125f;
    [Tooltip("Real-time seconds the game stays slowed down after a hit.")]
    [SerializeField] float hurtSlowdownDuration = .35f;
    [SerializeField] float hurtControlLoss = .2f;
    [SerializeField] ShakeSettings hurtShake = new ShakeSettings(.3f, 1f, 13);

    [Header("Death")]
    [SerializeField] GameObject deathParticles;
    [Tooltip("Seconds between dying and the game over screen.")]
    [SerializeField] float gameOverDelay = 2f;

    [Header("Dependencies")]
    [SerializeField] PlayerGraphics graphics;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerDash dash;
    [SerializeField] BombDefuser defuser;

    public UnityEvent OnHeal = new UnityEvent();
    public UnityEvent OnDamage = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsInvincible => Time.time < invincibleUntil;

    float invincibleUntil;
    int lastDamageFrame = -1;

    void Awake()
    {
        if (graphics == null) graphics = GetComponent<PlayerGraphics>();
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (dash == null) dash = GetComponent<PlayerDash>();
        if (defuser == null) defuser = GetComponentInChildren<BombDefuser>();

        CurrentHealth = maxHealth;
    }

    void Start()
    {
        GameEvents.RaisePlayerHealthChanged(CurrentHealth, maxHealth);
    }

    /// <summary>Makes the player immune to damage for at least this many seconds.</summary>
    public void GrantInvincibility(float duration)
    {
        invincibleUntil = Mathf.Max(invincibleUntil, Time.time + duration);
    }

    public void Heal(int amount = 1)
    {
        if (IsDead)
        {
            return;
        }

        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
        GameEvents.RaisePlayerHealthChanged(CurrentHealth, maxHealth);
        graphics.PlayHealthAnimation();
        OnHeal.Invoke();
    }

    public void TakeDamage(int amount = 1)
    {
        // One hit per frame at most, so overlapping hazards don't stack.
        if (IsDead || IsInvincible || lastDamageFrame == Time.frameCount)
        {
            return;
        }

        lastDamageFrame = Time.frameCount;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        GrantInvincibility(invincibilityTime);
        GameEvents.RaisePlayerHealthChanged(CurrentHealth, maxHealth);

        TimeScaleController.Hitstop(hurtTimeScale, hurtSlowdownDuration);
        CameraShaker.Shake(hurtShake);
        movement.LoseControl(hurtControlLoss);
        graphics.PlayHurtAnimation();
        SpawnParticles(damageParticles, 1f);
        OnDamage.Invoke();

        if (CurrentHealth == 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        IsDead = true;
        yield return new WaitForSecondsRealtime(hurtSlowdownDuration);

        SpawnParticles(deathParticles, 5f);
        graphics.PlayDeath();
        movement.enabled = false;
        if (dash != null) dash.enabled = false;
        if (defuser != null) defuser.enabled = false;
        OnDeath.Invoke();

        yield return new WaitForSeconds(gameOverDelay);
        GameEvents.RaisePlayerDied();
    }

    void SpawnParticles(GameObject prefab, float lifetime)
    {
        if (prefab != null)
        {
            Destroy(Instantiate(prefab, transform.position, Quaternion.identity), lifetime);
        }
    }
}
