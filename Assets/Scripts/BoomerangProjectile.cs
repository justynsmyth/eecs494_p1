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

    private bool isReturning = false;
    
    public void Setup(GameObject owner)
    {
        _owner = owner;
    }
    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        RoomTransition.Direction dir = RoomTransition.Direction.Up; // default 
        if (_owner.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            dir = GameObject.FindGameObjectWithTag("Player").GetComponent<InputToAnimator>()
                .GetPlayerDirection();
        }
        else
        {
            EnemyMovement em = _owner.GetComponent<EnemyMovement>();
            SpriteRenderer sr = em.GetSprite();
            Debug.Log(sr.sprite.name);
            if (sr.sprite == em.sideSprite || sr.sprite == em.sideSprite2)
            {
                if (sr.flipX) dir = RoomTransition.Direction.Left;
                else dir =RoomTransition.Direction.Right;
            }
            else if (sr.sprite == em.downSprite) dir = RoomTransition.Direction.Down;
            else dir = RoomTransition.Direction.Up;
        }
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
        }
        rb.linearVelocity = projectileDirection * projectileSpeed * 2;
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Throw");
        StartCoroutine(HandleReturn());
    }

    private IEnumerator HandleReturn()
    {
        yield return new WaitForSeconds(maxTravelTime);

        isReturning = true;
        projectileDirection = (_owner.transform.position - transform.position).normalized;
        rb.linearVelocity = projectileDirection * (projectileSpeed * returnSpeedMultiplier);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isReturning && other.gameObject == _owner)
        {
            OnReturn?.Invoke();
            Destroy(gameObject);
        } // Check that the other object is either an Enemy or Player, but not the owner
        else if ((other.CompareTag("Enemy") || other.CompareTag("Player")) && _owner != other.gameObject)
        {
            isReturning = true;
        }
    }

    void Update()
    {
        if (isReturning)
        {
            // Continuously update direction toward owner 
            projectileDirection = (_owner.transform.position - transform.position).normalized;
            rb.linearVelocity = projectileDirection * (projectileSpeed * returnSpeedMultiplier);
        }
    }
}