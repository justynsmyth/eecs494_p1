using UnityEngine;

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
    public Inventory inventory;
    public HasHealth player_health;

    private Vector2 currentRoom;
    private int numKills;

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

        numKills = 0;
    }

    void Update()
    {
        if (RoomTransition.isTransitionInProgress)
        {
            numKills = 0;
        }   
    }

    public void DropItem(int index, Vector3 location, float itemDropRate)
    {
        numKills++;

        if (numKills == 5)
        {
            Instantiate(key, location, Quaternion.identity);
        }
        else if (Random.Range(0.0f, 1.0f) < itemDropRate)
        {
            if (index == 0)
            {
                Instantiate(rupee, location, Quaternion.identity);
            }
            else if (index == 1)
            {
                Instantiate(heart, location, Quaternion.identity);
            }
        }
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
            Inventory.HasSword = true;
            Inventory.HasBow = true;
        }
        else
        {
            Debug.Log("god_mode off!");
            god_mode = false;
        }
    }
}
