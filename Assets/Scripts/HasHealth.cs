using Unity.VisualScripting;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public float health;
    private float maxHealth;

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
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    void Update()
    {
        if (health <= 0) 
        { 
            Destroy(gameObject);
        }    
    }
}
