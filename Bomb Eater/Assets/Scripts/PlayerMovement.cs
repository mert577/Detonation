using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float accelerationForce = 40f;

    Rigidbody2D rb;
    PlayerDash playerDash;
    bool controlLocked;

    public Vector2 Input { get; private set; }
    public bool CanControl => !controlLocked && GameManager.instance != null && GameManager.instance.gameState == GameState.Playing;

    public float dashCoolDownTimer
    {
        get => playerDash != null ? playerDash.CooldownTimer : 0f;
        set
        {
            if (playerDash != null)
            {
                playerDash.CooldownTimer = value;
            }
        }
    }

    public bool dashing => playerDash != null && playerDash.IsDashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        Input = CanControl
            ? Vector2.ClampMagnitude(new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical")), 1f)
            : Vector2.zero;
    }

    void FixedUpdate()
    {
        if (dashing)
        {
            return;
        }

        rb.AddForce(accelerationForce * Input);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    public void LoseControl(float time)
    {
        StartCoroutine(LoseControlRoutine(time));
    }

    IEnumerator LoseControlRoutine(float time)
    {
        controlLocked = true;
        yield return new WaitForSeconds(time);
        controlLocked = false;
    }
}
