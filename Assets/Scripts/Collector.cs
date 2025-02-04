using UnityEngine;

public class Collector : MonoBehaviour
{
    public AudioClip rupee_collection_sound_clip;
    public AudioClip heart_collection_sound_clip;
    public AudioClip key_collection_sound_clip;

    public AudioClip damage_item_collection_sound_clip;
    public AudioClip weapon_collection_sound_clip;

    public Sprite weaponPickupSprite;
    public float animationTime = 2f;

    Inventory inventory;
    HasHealth healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = GetComponent<Inventory>();
        healthBar = GetComponent<HasHealth>();
        if (inventory == null)
        {
            Debug.LogWarning("WARNING: Gameobject with a collector has no inventory to store things in!");
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        GameObject object_collided_with = coll.gameObject;

        if (object_collided_with.tag == "rupee")
        {
            // If our inventory exists, add a rupee to it
            if (inventory)
            {
                inventory.AddRupees(1);
            }

            Debug.Log("Collected rupee!");
            Destroy(object_collided_with);

            // Play sound effect
            AudioSource.PlayClipAtPoint(rupee_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "heart")
        {
            if (healthBar)
            {
                healthBar.AlterHealth(1);
            }

            Debug.Log("Collected heart!");
            Destroy(object_collided_with);

            AudioSource.PlayClipAtPoint(heart_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "key")
        {
            if (inventory)
            {
                inventory.AddKeys(1);
            }

            Debug.Log("Collected key!");
            Destroy(object_collided_with);

            // Play sound effect
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "bow")
        {
            inventory.UpdateZSlotItem("Bow"); 

            AudioSource.PlayClipAtPoint(weapon_collection_sound_clip, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(damage_item_collection_sound_clip, Camera.main.transform.position);

            Destroy(object_collided_with);

            //StartCoroutine(WeaponPickup(GetComponent<SpriteRenderer>(), object_collided_with));
        }
        else if (object_collided_with.tag == "bomb")
        {
            if (!object_collided_with.GetComponent<Bomb>().isDrop)
            {
                return;
            }

            if (inventory)
            {
                inventory.AddBomb(1);
                inventory.UpdateZSlotItem("Bomb");
            }
            
            Bomb b = object_collided_with.GetComponent<Bomb>();

            if (b != null && b.isDrop)
            {
                Destroy(object_collided_with);
            }

            AudioSource.PlayClipAtPoint(damage_item_collection_sound_clip, Camera.main.transform.position);
        } else if (object_collided_with.tag == "boomerang")
        {
            Boomerang b = object_collided_with.GetComponent<Boomerang>();
            
            if (b != null && !b.isDrop)
            {
                return; // ignores collision if boomerang is not a drop
            }

            if (inventory)
            {
                inventory.UpdateZSlotItem("Boomerang");
            }

            if (b != null && b.isDrop)
            {
                AudioSource.PlayClipAtPoint(weapon_collection_sound_clip, Camera.main.transform.position);
                AudioSource.PlayClipAtPoint(damage_item_collection_sound_clip, Camera.main.transform.position);
                Destroy(object_collided_with);
            }
            // play sound effect and animation
        }
    }

    /*
    IEnumerator WeaponPickup(SpriteRenderer playerSprite, GameObject item)
    {
        float timeElapsed = 0f;
        
        Sprite currentSprite = playerSprite.sprite;
        playerSprite.sprite = weaponPickupSprite;

        PlayerInput playerControl = GetComponent<PlayerInput>();
        playerControl.control = false;

        item.transform.position += Vector3.up * 1;

        while (timeElapsed < animationTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerSprite.sprite = currentSprite;
        playerControl.control = true;

        Destroy(item);
    }
    */
}
