using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class BoomerangProjectile : Projectile
{
    public event Action OnReturn;
    public float returnSpeedMultiplier = 0.5f; // Multiplier for speed when returning
    private GameObject _owner;
    public float maxTravelTime = 0.5f; // Time before the boomerang returns
    public AudioClip audio;

    private bool isReturning = false;
    
    public void Setup(GameObject owner)
    {
        _owner = owner;
    }
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        RoomTransition.Direction dir = DetermineInitialDirection();
        SetProjectileDirection(dir);
        LaunchProjectile();
        TriggerThrowAnimation();
        StartCoroutine(HandleReturn());
    }
    
    private RoomTransition.Direction DetermineInitialDirection()
    {
        if (_owner.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return GetPlayerDirection();
        }
        else
        {
            return GetEnemyDirection();
        }
    }
    
    private RoomTransition.Direction GetPlayerDirection()
    {
        InputToAnimator playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<InputToAnimator>();
        return playerAnimator.GetPlayerDirection();
    }

    private RoomTransition.Direction GetEnemyDirection()
    {
        EnemyMovement em = _owner.GetComponent<EnemyMovement>();
        SpriteRenderer sr = em.GetSprite();

        if ((sr.sprite == em.sideSprite || sr.sprite == em.sideSprite2) && sr.flipX)
        {
            return RoomTransition.Direction.Left;
        }
        if ((sr.sprite == em.sideSprite || sr.sprite == em.sideSprite2))
        {
            return RoomTransition.Direction.Right;
        }
        if (sr.sprite == em.downSprite)
        {
            return RoomTransition.Direction.Down;
        }
        return RoomTransition.Direction.Up;
    }
    
    private void SetProjectileDirection(RoomTransition.Direction dir)
    {
        switch (dir)
        {
            case RoomTransition.Direction.Up:
                projectileDirection = Vector2.up;
                break;
            case RoomTransition.Direction.Down:
                projectileDirection = Vector2.down;
                break;
            case RoomTransition.Direction.Left:
                projectileDirection = Vector2.left;
                break;
            case RoomTransition.Direction.Right:
                projectileDirection = Vector2.right;
                break;
            default:
                projectileDirection = Vector2.zero; // Ensure there's a safe default
                break;
        }
    }
    
    private void LaunchProjectile()
    {
        rb.linearVelocity = projectileDirection * projectileSpeed * 2;
    }
    
    private void TriggerThrowAnimation()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Throw");
    }


    private IEnumerator HandleReturn()
    {
        yield return new WaitForSeconds(maxTravelTime);

        isReturning = true;
        if (_owner != null)
        {
            projectileDirection = (_owner.transform.position - transform.position).normalized;
            rb.linearVelocity = projectileDirection * (projectileSpeed * returnSpeedMultiplier);
        }
        else
        {
            Debug.LogWarning("Boomerang has no valid owner to return to!");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isReturning && other.gameObject == _owner)
        {
            OnReturn?.Invoke();
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Enemy") && _owner.CompareTag("Player")) ||
                  ((other.CompareTag("Player") || other.CompareTag("Wall")) && _owner.CompareTag("Enemy") 
                 && _owner != other.gameObject))
        {
            isReturning = true;
        } else if (other.CompareTag("Weapon") && _owner.CompareTag("Enemy") && _owner != other.gameObject)
        {
            // If weapon collides with projectile, it will deflect
            isReturning = true;
            if (audio != null) {
                AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position);
            }
            Collider pcol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                Physics.IgnoreCollision(col, pcol, true);
            }
        }
    }

    void Update()
    {
        // if the owner is either not set or deleted, all projectiles must be deleted.
        if (_owner == null)
        {
            Destroy(gameObject);
            return;
        }
        if (isReturning)
        {
            // Continuously update direction toward owner 
            projectileDirection = (_owner.transform.position - transform.position).normalized;
            rb.linearVelocity = projectileDirection * (projectileSpeed * returnSpeedMultiplier);
            
            if (Vector3.Distance(_owner.transform.position, transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}