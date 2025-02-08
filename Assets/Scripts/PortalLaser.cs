using UnityEngine;


public class PortalLaser : MonoBehaviour
{
    public GameObject portalA;
    public GameObject portalB;

    public PortalGun portalGun;

    public static bool portalAorB = false;

    private bool used = false;
    private GameObject portalToCreate;

    private void OnCollisionEnter(Collision collision)
    {        
        // check for collisions with objects of default or block layer
        // comparetag needed for locked doors atm
        if ((collision.gameObject.layer == 0 || collision.gameObject.layer == 6) && !collision.gameObject.CompareTag("Doorway") &&!used)
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
        // this is if we hit another layer (i.e. doorways)
        else if (!used)
        {
            portalGun.SetNumPortals(-1);
        }
    }
}
