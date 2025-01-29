using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject openDoor;
    public Sprite openDoorSprite;
    public AudioClip openDoorSound;

    public bool verticalDoor;
    public GameObject otherDoor;
    public bool removeKey = true;
    public bool unlockableWithKey = true;

    private Inventory player_inventory;
    private bool locked = true;

    void Update()
    {
        if (verticalDoor && !locked)
        {
            UnlockDoor otherDoorUnlockScript = otherDoor.GetComponent<UnlockDoor>();

            Instantiate(otherDoorUnlockScript.openDoor, otherDoor.transform.position, Quaternion.identity);
            otherDoor.GetComponent<SpriteRenderer>().sprite = otherDoorUnlockScript.openDoorSprite;
            Destroy(otherDoor.gameObject.GetComponent<BoxCollider>());

            otherDoorUnlockScript.locked = false;

            if (otherDoorUnlockScript.removeKey && player_inventory)
            {
                player_inventory.AddKeys(-1);
            }

            // this part prevents the doors from continuing to spawn in the open door prefab
            otherDoorUnlockScript.verticalDoor = false;
            verticalDoor = false;
        }
    }

    public void DoorUnlock()
    {
        if (locked)
        {
            Instantiate(openDoor, gameObject.transform.position, Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
            Destroy(gameObject.GetComponent<BoxCollider>());

            locked = false;

            AudioSource.PlayClipAtPoint(openDoorSound, Camera.main.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!unlockableWithKey)
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Player") && locked)
        {
            player_inventory = collision.gameObject.GetComponent<Inventory>();

            if (player_inventory && player_inventory.GetKeys() > 0)
            {
                if (removeKey)
                {
                    player_inventory.AddKeys(-1);
                    removeKey = false;
                }
                Instantiate(openDoor, gameObject.transform.position, Quaternion.identity);
                gameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
                Destroy(gameObject.GetComponent<BoxCollider>());

                locked = false;

                AudioSource.PlayClipAtPoint(openDoorSound, Camera.main.transform.position);
            }
        }
    }
}
