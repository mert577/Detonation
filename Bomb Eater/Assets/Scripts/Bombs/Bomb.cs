using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum BombState
{
    Dropping,
    Armed,
    Exploded,
    Defused,
}

/// <summary>
/// The one bomb component. Everything type-specific comes from its <see cref="BombDefinition"/>.
/// Spawners call <see cref="Init"/> right after instantiating; a bomb placed by hand uses the definition set in the Inspector.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bomb : MonoBehaviour
{
    [SerializeField] BombDefinition definition;

    [Header("Parts")]
    [SerializeField] SpriteRenderer graphics;
    [SerializeField] Transform shadow;
    [SerializeField] BombMovement movement;

    [Header("Drop in")]
    [SerializeField] float dropHeight = 7f;
    [SerializeField] float dropTime = 1f;

    [Header("Warning")]
    [SerializeField] float warningShakeStrength = .25f;
    [SerializeField] int warningShakeVibrato = 15;

    public BombDefinition Definition => definition;
    public BombState State { get; private set; }
    public float TimeLeft { get; private set; }
    public bool CanBeDefused => State == BombState.Armed;
    public bool IsResolved => State == BombState.Exploded || State == BombState.Defused;

    BombModifiers modifiers = BombModifiers.Default;
    Rigidbody2D body;
    Collider2D[] colliders;
    bool warningStarted;

    public void Init(BombDefinition bombDefinition, BombModifiers bombModifiers)
    {
        definition = bombDefinition;
        modifiers = bombModifiers;
    }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        SetCollidersEnabled(false);
    }

    void Start()
    {
        if (definition == null)
        {
            Debug.LogError($"Bomb '{name}' has no BombDefinition.", this);
            Destroy(gameObject);
            return;
        }

        if (definition.sprite != null)
        {
            graphics.sprite = definition.sprite;
        }

        graphics.color = definition.tint;
        TimeLeft = definition.fuseTime;
        StartCoroutine(DropIn());
    }

    void Update()
    {
        shadow.localScale = Vector3.one * ((dropHeight - graphics.transform.localPosition.y) / dropHeight);

        if (State != BombState.Armed)
        {
            return;
        }

        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f)
        {
            Explode();
        }
        else if (!warningStarted && TimeLeft <= definition.warningTime)
        {
            StartWarning();
        }
    }

    IEnumerator DropIn()
    {
        State = BombState.Dropping;
        graphics.transform.localPosition = Vector2.up * dropHeight;
        graphics.transform.DOLocalMoveY(0f, dropTime).SetEase(Ease.OutBounce).SetLink(gameObject);
        yield return new WaitForSeconds(dropTime);

        State = BombState.Armed;
        SetCollidersEnabled(true);

        if (movement != null && definition.canMove && Random.value < modifiers.movementChance)
        {
            movement.StartMoving(definition.moveSpeed);
        }
    }

    void StartWarning()
    {
        warningStarted = true;
        graphics.transform.DOShakePosition(TimeLeft, warningShakeStrength, warningShakeVibrato).SetLink(gameObject);
        graphics.DOColor(Color.white, TimeLeft).SetEase(Ease.OutQuad).SetLink(gameObject);
    }

    public void Explode()
    {
        if (IsResolved)
        {
            return;
        }

        State = BombState.Exploded;
        CameraShaker.Shake(definition.explodeShake);
        graphics.gameObject.SetActive(false);
        shadow.gameObject.SetActive(false);
        SetCollidersEnabled(false);

        if (movement != null)
        {
            movement.StopMoving();
        }

        body.bodyType = RigidbodyType2D.Kinematic;
        body.linearVelocity = Vector2.zero;

        SpawnParticle(definition.explodeParticle);
        GameEvents.RaiseBombExploded(this);
        StartCoroutine(PlayPatterns());
    }

    public void Defuse()
    {
        if (!CanBeDefused)
        {
            return;
        }

        State = BombState.Defused;
        CameraShaker.Shake(definition.defuseShake);
        SpawnParticle(definition.defuseParticle);
        GameEvents.RaiseBombDefused(this);
        Destroy(gameObject);
    }

    IEnumerator PlayPatterns()
    {
        Vector2 origin = transform.position;
        Transform player = PlayerLocator.Transform;
        Vector2 target = player != null ? (Vector2)player.position : origin + Vector2.right;
        var context = new ExplosionContext(origin, target, modifiers.projectileSpeedMultiplier);

        var running = new List<Coroutine>();
        foreach (ExplosionPattern pattern in definition.patterns)
        {
            if (pattern != null)
            {
                running.Add(StartCoroutine(pattern.Play(context)));
            }
        }

        foreach (Coroutine coroutine in running)
        {
            yield return coroutine;
        }

        Destroy(gameObject);
    }

    void SpawnParticle(GameObject prefab)
    {
        if (prefab != null)
        {
            Destroy(Instantiate(prefab, transform.position, Quaternion.identity), 1f);
        }
    }

    void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider2D bombCollider in colliders)
        {
            bombCollider.enabled = enabled;
        }
    }
}
