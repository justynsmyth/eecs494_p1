using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public AudioClip gameOverSound;
    public AudioClip playerPopSound;
    
    public int maxEnemies = 2;
    public int enemies = 0;

    public float gameOverPauseTime = 0.5f;
    public float gameOverAnimateTime = 0.15f;

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

    public IEnumerator GameOver(GameObject player)
    {
        GameObject.Find("Main Camera").GetComponent<AudioSource>().enabled = false;
        god_mode = true;
        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<InputToAnimator>().ToggleMovement();
        player.GetComponent<Rigidbody>().linearVelocity = Vector2.zero;

        float startTime = Time.time;
        while (Time.time - startTime < gameOverPauseTime)
        {
            yield return null;
        }

        AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position);

        yield return StartCoroutine(player.GetComponent<InputToAnimator>().GameOverAnimation(gameOverAnimateTime));

        startTime = Time.time;
        while (Time.time - startTime < gameOverPauseTime)
        {
            yield return null;
        }

        Destroy(player);
        AudioSource.PlayClipAtPoint(playerPopSound, Camera.main.transform.position);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        god_mode = false;
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
            inventory.UnlockAllWeapons();
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
