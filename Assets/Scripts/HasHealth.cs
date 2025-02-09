using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasHealth : MonoBehaviour
{
    public float health;
    private float maxHealth;

    public float itemDropRate = 0.4f;
    public RoomClear room;
    public float stunDuration = 3f;

    public AudioClip damageAudio;

    public float invulnerability_duration = 1.0f;
    public bool IsInvulnerable = false;
    public bool player_iframes = false;
    private float playerControlLossTime = 0.25f;
    private bool alive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    public void AlterHealth(float num_health)
    {
        if (health + num_health <= maxHealth)
        {
            health += num_health;
            Debug.Log(health);
        }
        else
        {
            MaximizeHealth();
        }

        if (num_health < 0)
        {
            AudioSource.PlayClipAtPoint(damageAudio, Camera.main.transform.position);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void MaximizeHealth()
    {
        health = maxHealth;
    }

    public bool HasMaxHealth()
    {
        return health == maxHealth;
    }

    void Update()
    {
        if (health <= 0 && alive) 
        { 
            alive = false;
            Vector3 position = gameObject.transform.position;

            // if player dies, game over and restart the game
            if (gameObject.tag == "Player")
            {
                StartCoroutine(GameManager.instance.GameOver(gameObject));
            }
            else
            {
                Destroy(gameObject);
            }

            // if enemy dies, potentially drops an item
            if (gameObject.tag == "Enemy")
            {
                room.EnemyDeath();
                GameManager.instance.DropItem(Random.Range(0, 3), position, itemDropRate);
            }
        }    
    }

    public IEnumerator InvulnerabilityCooldown()
    {
        IsInvulnerable = true;
        StartCoroutine(EnemyHitFlash(GetComponent<SpriteRenderer>()));
        yield return new WaitForSeconds(invulnerability_duration);
        IsInvulnerable = false;
    }

    public IEnumerator EnemyHitFlash(SpriteRenderer enemySprite)
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

    public void PlayerHit()
    {
        StartCoroutine(PlayerIFrames());
    }

    private IEnumerator PlayerIFrames()
    {
        PlayerInput player = gameObject.GetComponent<PlayerInput>();
        SpriteRenderer currentSprite = gameObject.GetComponent<SpriteRenderer>();

        float timeElapsed = 0f;
        player_iframes = true;
        player.control = false;
        currentSprite.color = Color.red;

        while (timeElapsed < invulnerability_duration)
        {
            if (timeElapsed > playerControlLossTime)
            {
                player.control = true;
                currentSprite.color = Color.white;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        player_iframes = false;
        player.control = true;
        currentSprite.color = Color.white;
    }

    public IEnumerator StunCooldown()
    {
        EnemyMovement em = GetComponent<EnemyMovement>();
        em.stopMovement = true;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(stunDuration);
        em.stopMovement = false;
    }
}
