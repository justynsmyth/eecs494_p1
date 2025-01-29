using UnityEngine;

public class ShutDoor : MonoBehaviour
{
    public GameObject door;
    public Sprite closedDoor;

    private LayerMask layers;
    private bool closed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !closed) {
            door.GetComponent<SpriteRenderer>().sprite = closedDoor;
            door.GetComponent<BoxCollider>().excludeLayers = layers;
            closed = true;
        }
    }
}
