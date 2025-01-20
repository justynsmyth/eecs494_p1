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
    public Inventory inventory;
    public HasHealth player_health;

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

    public void DropItem(int index, Vector3 location)
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

    public void CheatModeToggle()
    {   
        // if turning on god_mode, maximize player resources and HP
        if (!god_mode)
        {
            Debug.Log("god_mode on!");
            god_mode = true;
            inventory.MaximizeResources(999);
            player_health.MaximizeHealth();
        }
        else
        {
            Debug.Log("god_mode off!");
            god_mode = false;
        }
    }
}
