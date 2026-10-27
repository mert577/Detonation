using UnityEngine;

/// <summary>
/// Hurts the player on contact, through collisions or triggers. The player's invincibility frames limit the rate.
/// </summary>
public class Damager : MonoBehaviour
{
    [Min(1)] [SerializeField] int damage = 1;

    void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision.gameObject);

    void OnCollisionStay2D(Collision2D collision) => TryDamage(collision.gameObject);

    void OnTriggerEnter2D(Collider2D collision) => TryDamage(collision.gameObject);

    void OnTriggerStay2D(Collider2D collision) => TryDamage(collision.gameObject);

    void TryDamage(GameObject other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
