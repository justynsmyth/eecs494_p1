using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject rupee;
    public GameObject heart;
    public GameObject key;
    public GameObject bomb;
    public Inventory inventory;
    public HasHealth player_health;
    public GameObject enemyPrefab;
    
    
    public int maxEnemies = 2;
    public int enemies = 0;

    private List<Vector3> usedSpawnLocations = new List<Vector3>();

    public static bool god_mode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1024, 960, false);
        if (player_health == null)
        {
            Debug.LogError("player_health is null");
        }
        if (inventory == null) { Debug.LogError("inventory is null"); }
        if (rupee == null) { Debug.LogError("rupee is null"); }
        if (heart == null) { Debug.LogError("heart is null"); }
    }

    public void DropItem(int index, Vector3 location, float itemDropRate)
    {
        if (Random.Range(0.0f, 1.0f) < itemDropRate)
        {
            if (index == 0)
            {
                Instantiate(rupee, location, Quaternion.identity);
            }
            else if (index == 1)
            {
                Instantiate(heart, location, Quaternion.identity);
            }
            else
            {
                Instantiate(bomb, location, Quaternion.identity);
            }
        }
    }
    
    public bool SpawnEnemy(Vector3 spawnLocation, Vector3 initDirection, Vector3 turnDirection, float speed)
    {
        if (enemies == maxEnemies) return false;
        
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab not assigned!");
            return false;
        }

        if (usedSpawnLocations.Contains(spawnLocation)) return false;

        GameObject enemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
        enemies += 1;
        usedSpawnLocations.Add(spawnLocation);
        
        enemy.GetComponent<WallMasterMover>().Initialize(initDirection, turnDirection, speed);
        return true;
    } 

    public void CheatModeToggle()
    {   
        // if turning on god_mode, maximize player resources and HP
        if (!god_mode)
        {
            Debug.Log("god_mode on!");
            god_mode = true;
            inventory.MaximizeResources();
            player_health.MaximizeHealth();
            inventory.UpdateXSlotItem("Sword");
            inventory.UpdateZSlotItem("Bow");
        }
        else
        {
            Debug.Log("god_mode off!");
            god_mode = false;
        }
    }
}
