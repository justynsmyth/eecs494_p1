using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public Vector2 projectileDirection = Vector2.right;
    public float projectileLifeTime = 1f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = projectileDirection * projectileSpeed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("heart") || collision.collider.CompareTag("rupee"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else
        {
            TriggerExplosion(projectileLifeTime);
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

    protected virtual void TriggerExplosion(float lifeTime)
    {
        
    }
   
}

