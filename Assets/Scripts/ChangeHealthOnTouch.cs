using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class ChangeHealthOnTouch : MonoBehaviour
{
    public float health_change_amount;
    public float knockback_power;
    public bool destroy_self_on_touch = false;

    private bool player_iframes = false;

    public void OnTriggerEnter(Collider other)
    {
        // if Player is collided with and in god mode, do nothing
        if (other.gameObject.CompareTag("Player") && (GameManager.god_mode || player_iframes))
        {
            return;
        }

        /* Adjust hp */
        HasHealth other_health = other.GetComponent<HasHealth>();
        if (other_health == null)
            return;

        other_health.AlterHealth(health_change_amount);

        /* Perform Knockback */
        Rigidbody other_rb = other.GetComponent<Rigidbody>();
        if (other_rb != null)
        {
            Vector3 knockback_direction = (other.transform.position - transform.position).normalized;
            Debug.Log(knockback_direction.normalized);
            other_rb.linearVelocity = knockback_direction * knockback_power;
            Debug.Log(other_rb.linearVelocity);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInput playerInput = other.gameObject.GetComponent<PlayerInput>();
            SpriteRenderer playerSprite = other.gameObject.GetComponent<SpriteRenderer>();
            StartCoroutine(PlayerHit(playerInput, playerSprite));
        }

        /* Destroy self */
        if (destroy_self_on_touch)
            Destroy(gameObject);
    }

    IEnumerator PlayerHit(PlayerInput player, SpriteRenderer currentSprite)
    {
        float timeElapsed = 0f;
        player_iframes = true;
        player.control = false;

        Debug.Log("Player was hit");
        while (timeElapsed < 2f)
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

            if (timeElapsed > 0.5f)
            {
                player.control = true;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        player_iframes = false;
        player.control = true;
        currentSprite.color = Color.white;

        Debug.Log("Player out of invincibility");
    }
}