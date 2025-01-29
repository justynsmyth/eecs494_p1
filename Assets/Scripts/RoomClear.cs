using UnityEngine;

public class RoomClear : MonoBehaviour
{
    public Sprite lockedDoor;

    public Vector2 dropItem;
    public GameObject item;

    private int enemyCount = 0;
    private GameObject room;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        room = gameObject;

        if (room != null)
        {
            foreach (Transform enemy in room.transform)
            {
                if (enemy.gameObject.CompareTag("Enemy"))
                {
                    enemyCount++;
                }
            }
        }

        Debug.Log("number of enemies: " + enemyCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            RoomCleared();
        }
    }

    void RoomCleared()
    {
        if (item != null)
        {
            Instantiate(item, dropItem, Quaternion.identity);
        }
    }
}
