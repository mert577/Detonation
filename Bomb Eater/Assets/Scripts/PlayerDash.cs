using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
public class PlayerDash : MonoBehaviour
{
    const float DashInvincibilityBuffer = 0.12f;

    [Header("Dash")]
    [SerializeField] float dashForce = 20f;
    [SerializeField] float dashDrag = 6f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 1.2f;
    [SerializeField] float dashRenewParticleLeadTime = 0.15f;
    [SerializeField] int maxDashes = 1;
    [SerializeField] float dashInputBufferTime = 0.12f;

    [Header("Dependencies")]
    [SerializeField] BombDefuser defuser;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerDashVisuals visuals;

    Rigidbody2D rb;
    PlayerMovement movement;

    public bool IsDashing { get; private set; }
    public float CooldownTimer { get; set; }
    public int DashesLeft { get; private set; }
    float dashInputBufferTimer;
    bool playedRenewParticleThisCooldown;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();

        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (defuser == null)
        {
            defuser = GetComponentInChildren<BombDefuser>();
        }

        if (visuals == null)
        {
            visuals = GetComponent<PlayerDashVisuals>();
        }
    }

    void Start()
    {
        DashesLeft = maxDashes;
    }

    void Update()
    {
        HandleInputBuffer();
        HandleCooldown();

        if (!CanDashNow())
        {
            return;
        }

        StartCoroutine(DashRoutine());
    }

    bool CanDashNow()
    {
        return !IsDashing
            && movement != null
            && movement.CanControl
            && CooldownTimer <= 0f
            && dashInputBufferTimer > 0f
            && DashesLeft > 0;
    }

    void HandleInputBuffer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            dashInputBufferTimer = dashInputBufferTime;
            return;
        }

        dashInputBufferTimer = Mathf.Max(0f, dashInputBufferTimer - Time.deltaTime);
    }

    void HandleCooldown()
    {
        float oldTime = CooldownTimer;
        CooldownTimer = Mathf.Max(0f, CooldownTimer - Time.deltaTime);

        float warningTime = Mathf.Max(0f, dashRenewParticleLeadTime);
        if (!playedRenewParticleThisCooldown && oldTime >= warningTime && CooldownTimer < warningTime)
        {
            playedRenewParticleThisCooldown = true;
            if (visuals != null)
            {
                visuals.PlayDashReady();
            }
        }

        if (oldTime > 0f && CooldownTimer <= 0f)
        {
            DashesLeft = maxDashes;
        }
    }

    IEnumerator DashRoutine()
    {
        dashInputBufferTimer = 0f;

        if (playerHealth != null)
        {
            playerHealth.invincibilityTimer = dashTime + DashInvincibilityBuffer;
        }

        IsDashing = true;
        DashesLeft--;

        if (DashesLeft <= 0)
        {
            DashesLeft = 0;
            CooldownTimer = dashCooldown;
            playedRenewParticleThisCooldown = false;
        }

        if (visuals != null)
        {
            visuals.PlayDashStart(dashTime);
        }

        if (defuser != null)
        {
            defuser.ableToDefuse = true;
        }

        float initialDrag = rb.linearDamping;
        rb.linearDamping = dashDrag;
        rb.linearVelocity = GetDashDirection() * dashForce;

        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            rb.linearDamping = Mathf.Lerp(dashDrag, initialDrag, elapsed / dashTime);
            yield return null;
        }

        rb.linearDamping = initialDrag;
        IsDashing = false;

        if (defuser != null)
        {
            defuser.ableToDefuse = false;
        }

        if (visuals != null)
        {
            visuals.PlayDashEnd();
        }
    }

    Vector2 GetDashDirection()
    {
        if (defuser != null && defuser.closestBomb != null)
        {
            return (defuser.closestBomb.position - transform.position).normalized;
        }

        Vector2 input = movement != null ? movement.Input : Vector2.zero;
        if (input.sqrMagnitude > 0f)
        {
            return input.normalized;
        }

        return transform.up;
    }
 
    public void RestoreDashesFromDefuse()
    {
        DashesLeft = maxDashes;
        CooldownTimer = 0f;
        playedRenewParticleThisCooldown = false;
    }
}
