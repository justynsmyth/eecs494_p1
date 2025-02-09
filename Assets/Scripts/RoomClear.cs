using UnityEngine;

public class RoomClear : MonoBehaviour
{
    public UnlockDoor lockedDoor;

    public Vector2 dropItem;
    public GameObject item;
    public AudioClip itemDropAudio;

    private int enemyCount = 0;
    private GameObject room;
    private bool cleared = false;

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
            AudioSource.PlayClipAtPoint(itemDropAudio, Camera.main.transform.position);
            Instantiate(item, dropItem, Quaternion.identity);
        }

        if (lockedDoor != null)
        {
            lockedDoor.DoorUnlock();
        }

        cleared = true;
    }

    public bool CheckIfCleared()
    {
        return cleared;
    }
}
