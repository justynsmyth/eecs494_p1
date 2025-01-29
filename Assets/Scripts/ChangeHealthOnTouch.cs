using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class ChangeHealthOnTouch : MonoBehaviour
{
    public float healthInflictOnTouch = -0.5f;
    public float healthDamagedOnTouch = -1f;
    public float knockback_power;
    public bool destroy_self_on_touch = false;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Collider selfCollider = GetComponent<Collider>();
        // if Player is collided with and in god mode, do nothing
        if (other.gameObject.CompareTag("Player")) 
        {
            HasHealth playerHealth = other.gameObject.GetComponent<HasHealth>();
            if (GameManager.god_mode || playerHealth.player_iframes)
            {
                return;
            }
            // TODO: May need to do a null check here
            playerHealth.PlayerHit();
            HandleHealthAndKnockback(selfCollider, other, healthInflictOnTouch);
        } else if (other.gameObject.CompareTag("Weapon") && gameObject.CompareTag("Enemy"))
        {
            HasHealth enemyHealth = gameObject.GetComponent<HasHealth>();
            if (enemyHealth.IsInvulnerable) return;
            StartCoroutine(enemyHealth.InvulnerabilityCooldown()); 
            HandleHealthAndKnockback(other, selfCollider, healthDamagedOnTouch);
        }

        /* Destroy self */
        if (destroy_self_on_touch)
        {
            Destroy(gameObject);
        }
    }

    private void HandleHealthAndKnockback(Collider attacker, Collider target, float damage)
    {
        /* Adjust hp */
        HasHealth target_health = target.GetComponent<HasHealth>();
        if (target_health != null) { target_health.AlterHealth(damage); }
    
        /* Perform Knockback */
        Rigidbody target_rb = target.GetComponent<Rigidbody>();
        if (target_rb != null)
        {
            Vector3 direction3D = target.transform.position -  attacker.transform.position;
            Vector2 direction = new Vector2(direction3D.x, direction3D.y).normalized;
            Vector2 knockback_direction;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                knockback_direction = (direction.x > 0) ? Vector2.right : Vector2.left;
            }
            else
            {
                knockback_direction = (direction.y > 0) ? Vector2.up : Vector2.down;
            }
            target_rb.linearVelocity= knockback_direction * knockback_power;
        }
    } 
}