using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class ChangeHealthOnTouch : MonoBehaviour
{
    public float healthInflictOnTouch = -0.5f;
    public float healthDamagedOnTouch = -1f;
    public float knockback_power;
    public bool destroy_self_on_touch = false;
    public float invulnerability_duration = 1.5f;

    private bool player_iframes = false;
    
    public bool IsInvulnerable = false;

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
            if (GameManager.god_mode || player_iframes)
            {
                return;
            }
            PlayerInput playerInput = other.gameObject.GetComponent<PlayerInput>();
            SpriteRenderer playerSprite = other.gameObject.GetComponent<SpriteRenderer>();
            // ! May need to do a null check here
            StartCoroutine(PlayerHit(playerInput, playerSprite));
            HandleHealthAndKnockback(selfCollider, other, healthInflictOnTouch);
        } else if (other.gameObject.CompareTag("Weapon"))
        {
            if (IsInvulnerable) return;
            StartCoroutine(InvulnerabilityCooldown()); 
            HandleHealthAndKnockback(other, selfCollider, healthDamagedOnTouch);

        }


        /* Destroy self */
        if (destroy_self_on_touch)
            Destroy(gameObject);
    }

    private IEnumerator InvulnerabilityCooldown()
    {
        IsInvulnerable = true;
        StartCoroutine(EnemyHitFlash(sr));
        yield return new WaitForSeconds(invulnerability_duration);
        IsInvulnerable = false;
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

    IEnumerator PlayerHit(PlayerInput player, SpriteRenderer currentSprite)
    {
        float timeElapsed = 0f;
        player_iframes = true;
        player.control = false;

        while (timeElapsed < invulnerability_duration)
        {
            if (currentSprite.color == Color.red)
            {
                currentSprite.color = Color.blue;
            }
            else if (currentSprite.color == Color.blue)
            {
                currentSprite.color = Color.white;
            }
            else
            {
                currentSprite.color = Color.red;
            }

            if (timeElapsed > invulnerability_duration)
            {
                player.control = true;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        player_iframes = false;
        player.control = true;
        currentSprite.color = Color.white;
    }
    
    private IEnumerator EnemyHitFlash(SpriteRenderer enemySprite)
    {
        float timeElapsed = 0f;
        while (timeElapsed < invulnerability_duration)
        {
            if (enemySprite.color == Color.red)
            {
                enemySprite.color = Color.blue;
            }
            else if (enemySprite.color == Color.blue)
            {
                enemySprite.color = Color.white;
            }
            else
            {
                enemySprite.color = Color.red;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        enemySprite.color = Color.white; // Reset color after flashing
    }
}