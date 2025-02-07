using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalLaser : MonoBehaviour
{
    public GameObject portalA;
    public GameObject portalB;

    public static bool portalAorB = false;

    private bool used = false;
    private GameObject portalToCreate;

    private void OnCollisionEnter(Collision collision)
    {
        // check for collisions with objects of default or block layer
        if ((collision.gameObject.layer == 0 || collision.gameObject.layer == 6) && !used)
        {
            Debug.Log("Collided with wall");
            // create a portal
            if (!portalAorB)
            {
                portalToCreate = portalA;
                portalAorB = true;
            }
            else
            {
                portalToCreate = portalB;
                portalAorB = false;
            }

            GameObject newPortal = Instantiate(portalToCreate, collision.transform.position, Quaternion.identity);
            Portal newPortalScript = newPortal.GetComponent<Portal>();
            newPortalScript.offset = (gameObject.GetComponent<Projectile>().projectileDirection * -1).normalized;

            used = true;
        }
    }
}
