using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public Vector2 projectileDirection = Vector2.right;
    public float projectileLifeTime = 1f;
    protected Rigidbody rb;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateMovement();
    }

    public void UpdateMovement()
    {
        rb.linearVelocity = projectileDirection * projectileSpeed;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("heart") || collision.collider.CompareTag("rupee"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else
        {
            TriggerImpactAction(projectileLifeTime);
            HideProjectile();
            Destroy(gameObject, projectileLifeTime + 0.1f);
        }
    }
    
    private void HideProjectile()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    // Use as calling function in inherited class when projectile collides
    protected virtual void TriggerImpactAction(float lifeTime)
    {
       // implemented in inherited classes 
    }
   
}

