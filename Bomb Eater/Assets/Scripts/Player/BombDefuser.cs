using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// The player's defuse range. Tracks the closest bomb (dashes aim at it), defuses bombs touched while dashing,
/// and bounces the player off bombs touched otherwise.
/// </summary>
public class BombDefuser : MonoBehaviour
{
    [FormerlySerializedAs("layer")]
    [SerializeField] LayerMask bombLayer;
    [SerializeField] float defuseRange = 2.5f;
    [SerializeField] SpriteRenderer rangeSprite;

    [Header("Target marker")]
    [Tooltip("Shown on the closest bomb in range.")]
    [FormerlySerializedAs("target")]
    [SerializeField] GameObject targetMarker;
    [SerializeField] float markerFollowSharpness = 12f;

    [Header("Defuse feedback")]
    [SerializeField] float defuseTimeScale = .05f;
    [SerializeField] float defuseSlowdownDuration = .06f;

    [Header("Bump")]
    [SerializeField] float bumpForce = 80f;
    [SerializeField] float bumpControlLoss = .1f;

    [Header("Dependencies")]
    [SerializeField] PlayerMovement movement;
    [SerializeField] Rigidbody2D body;

    public bool IsDefusing { get; private set; }
    public Transform ClosestBomb { get; private set; }

    readonly List<Collider2D> bombsInRange = new List<Collider2D>();
    ContactFilter2D bombFilter;

    void Awake()
    {
        if (movement == null) movement = GetComponentInParent<PlayerMovement>();
        if (body == null) body = GetComponentInParent<Rigidbody2D>();

        bombFilter = new ContactFilter2D { useTriggers = true };
        bombFilter.SetLayerMask(bombLayer);
    }

    void Start()
    {
        rangeSprite.transform.localScale = new Vector3(defuseRange * 2f, defuseRange * 2f, 1f);
    }

    void OnDisable()
    {
        IsDefusing = false;
        ClosestBomb = null;

        // The marker lives outside the player and may already be gone when the scene unloads.
        if (targetMarker != null)
        {
            targetMarker.SetActive(false);
        }
    }

    public void SetDefusing(bool defusing)
    {
        IsDefusing = defusing;
    }

    void Update()
    {
        ClosestBomb = FindClosestBomb();
        UpdateTargetMarker();
    }

    Transform FindClosestBomb()
    {
        Physics2D.OverlapCircle(transform.position, defuseRange, bombFilter, bombsInRange);

        Transform closest = null;
        float closestDistance = float.MaxValue;
        foreach (Collider2D bombCollider in bombsInRange)
        {
            float distance = ((Vector2)(bombCollider.transform.position - transform.position)).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = bombCollider.transform;
            }
        }

        return closest;
    }

    void UpdateTargetMarker()
    {
        Transform marker = targetMarker.transform;
        if (ClosestBomb == null)
        {
            targetMarker.SetActive(false);
            marker.position = transform.position;
            return;
        }

        targetMarker.SetActive(true);
        float follow = 1f - Mathf.Exp(-markerFollowSharpness * Time.deltaTime);
        marker.position = Vector3.Lerp(marker.position, ClosestBomb.position, follow);

        Vector2 away = (ClosestBomb.position - transform.position).normalized;
        marker.right = Quaternion.Euler(0f, 0f, 45f) * away;
    }

    void OnTriggerEnter2D(Collider2D collision) => HandleContact(collision);

    void OnTriggerStay2D(Collider2D collision) => HandleContact(collision);

    void HandleContact(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Bomb bomb) || !bomb.CanBeDefused)
        {
            return;
        }

        if (IsDefusing)
        {
            TimeScaleController.Hitstop(defuseTimeScale, defuseSlowdownDuration, true);
            bomb.Defuse();
            return;
        }

        Vector2 away = (bomb.transform.position - transform.position).normalized;
        movement.LoseControl(bumpControlLoss);
        body.AddForce(-away * bumpForce, ForceMode2D.Impulse);
    }
}
