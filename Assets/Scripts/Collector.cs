using UnityEngine;

public class Collector : MonoBehaviour
{
    public AudioClip rupee_collection_sound_clip;
    public AudioClip heart_collection_sound_clip;
    public AudioClip key_collection_sound_clip;

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

            AudioSource.PlayClipAtPoint(heart_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "key")
        {
            if (inventory)
            {
                inventory.AddKeys(1);
            }

            Debug.Log("Collected key!");
            
            AudioSource.PlayClipAtPoint(key_collection_sound_clip, Camera.main.transform.position);
        }
        else if (object_collided_with.tag == "bow")
        {
            Inventory.HasBow = true;

            // play sound effect and animation
        }

        Destroy(object_collided_with);
    }
}
