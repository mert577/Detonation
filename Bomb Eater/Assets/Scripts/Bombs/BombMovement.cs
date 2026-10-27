using UnityEngine;

/// <summary>
/// Makes a bomb wander in a straight line and bounce off walls. Speed comes from the bomb's definition.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BombMovement : MonoBehaviour
{
    Rigidbody2D body;
    Vector2 velocity;

    public bool IsMoving { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void StartMoving(float speed)
    {
        IsMoving = true;
        velocity = UtilityClass.GetDirectionFromRotation(Random.Range(0f, 360f)) * speed * Random.Range(.7f, 1f);
        body.linearVelocity = velocity;
    }

    public void StopMoving()
    {
        if (!IsMoving)
        {
            return;
        }

        IsMoving = false;
        velocity = Vector2.zero;
        body.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsMoving)
        {
            return;
        }

        velocity = Vector2.Reflect(velocity, collision.GetContact(0).normal);
        body.linearVelocity = velocity;
    }
}
