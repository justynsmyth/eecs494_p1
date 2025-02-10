using UnityEditor.SceneManagement;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject openDoor;
    public Sprite openDoorSprite;
    public AudioClip openDoorSound;

    public Sprite closedDoorSprite;
    public GameObject closedDoor;
    
    public bool verticalDoor;
    public GameObject otherDoor;
    public bool removeKey = true;
    public bool unlockableWithKey = true;

    private Inventory player_inventory;
    private bool locked = true;

    private void Start()
    {
        player_inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
    }

    void Update()
    {
        if (verticalDoor && !locked)
        {
            UnlockDoor otherDoorUnlockScript = otherDoor.GetComponent<UnlockDoor>();

            otherDoorUnlockScript.DoorUnlock();

            // this part prevents the doors from continuing to call DoorUnlock()
            otherDoorUnlockScript.verticalDoor = false;
            verticalDoor = false;
        }
    }

    public void DoorUnlock()
    {
        if (locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = openDoorSprite;
            gameObject.GetComponent<BoxCollider>().center = openDoor.GetComponent<BoxCollider>().center;
            gameObject.GetComponent<BoxCollider>().size = openDoor.GetComponent<BoxCollider>().size;
            gameObject.GetComponent<BoxCollider>().excludeLayers = openDoor.GetComponent<BoxCollider>().excludeLayers;

            locked = false;

            AudioSource.PlayClipAtPoint(openDoorSound, Camera.main.transform.position);

            if (removeKey)
            {
                player_inventory.AddKeys(-1);
                removeKey = false;
            }
        }
    }

    public void DoorLock()
    {
        if (closedDoor == null || closedDoorSprite == null) return;
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closedDoorSprite;
            gameObject.GetComponent<BoxCollider>().center = closedDoor.GetComponent<BoxCollider>().center;
            gameObject.GetComponent<BoxCollider>().size = closedDoor.GetComponent<BoxCollider>().size;
            gameObject.GetComponent<BoxCollider>().excludeLayers = new LayerMask();
            AudioSource.PlayClipAtPoint(openDoorSound, Camera.main.transform.position);
            locked = true;
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
            if (player_inventory && player_inventory.GetKeys() > 0)
            {
                DoorUnlock();
            }
        }
    }
}
