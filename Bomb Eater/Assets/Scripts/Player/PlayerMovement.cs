using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float accelerationForce = 40f;

    Rigidbody2D rb;
    PlayerDash playerDash;
    float controlLockedUntil;

    public Vector2 MoveInput { get; private set; }
    public bool CanControl => Time.time >= controlLockedUntil
        && GameManager.instance != null
        && GameManager.instance.State == GameState.Playing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        MoveInput = CanControl ? GameInput.Move : Vector2.zero;
    }

    void FixedUpdate()
    {
        if (playerDash != null && playerDash.IsDashing)
        {
            return;
        }

        rb.AddForce(accelerationForce * MoveInput);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    /// <summary>Ignores input for a while. Overlapping calls keep the longest lock.</summary>
    public void LoseControl(float duration)
    {
        controlLockedUntil = Mathf.Max(controlLockedUntil, Time.time + duration);
    }
}
