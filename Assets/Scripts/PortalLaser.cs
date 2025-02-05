using UnityEngine;

public class PortalLaser : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // check for collisions with objects of default or block layer
        if (collision.gameObject.layer == 0 ||  collision.gameObject.layer == 6)
        {
            Debug.Log("Collided with wall");
            // create a portal
        }
    }
}
