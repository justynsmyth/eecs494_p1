using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasHealth : MonoBehaviour
{

    public float health;
    private float maxHealth;

    public float itemDropRate = 0.4f;

    public float invulnerability_duration = 1.0f;
    public bool IsInvulnerable = false;
    public bool player_iframes = false;
    private float playerControlLossTime = 0.25f;

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
        if (health <= 0) 
        { 
            Vector3 position = gameObject.transform.position;
            Destroy(gameObject);

            // if player dies, game over and restart the game
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            // if enemy dies, potentially drops an item
            if (gameObject.tag == "Enemy")
            {
                GameManager.instance.DropItem(Random.Range(0, 2), position, itemDropRate);
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
        Debug.Log("Started being hit");
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
            Debug.Log("Middle of being hit");
            yield return null;
        }
        Debug.Log("Finished being hit");
        player_iframes = false;
        player.control = true;
        currentSprite.color = Color.white;
    }
}
