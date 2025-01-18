using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.FilePathAttribute;

public class HasHealth : MonoBehaviour
{
    public GameManager manager;

    public float health;
    private float maxHealth;

    private float itemDropRate = 1.0f;

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

    void Update()
    {
        if (health <= 0) 
        { 
            Destroy(gameObject);

            // if player dies, game over and restart the game
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            // if enemy dies, potentially drops an item
            if (gameObject.tag == "Enemy")
            {
                if (Random.Range(0.0f, 1.0f) < itemDropRate)
                {
                    manager.DropItem(Random.Range(0, 2), gameObject.transform.position);
                }
            }
        }    
    }
}
