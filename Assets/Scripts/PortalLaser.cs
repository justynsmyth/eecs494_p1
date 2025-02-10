using System;
using Unity.VisualScripting;
using UnityEngine;


public class PortalLaser : MonoBehaviour
{
    public GameObject portalA;
    public GameObject portalB;
    
    public PortalGun portalGun;

    public static bool isNextPortalA = true; // spawns portal A first
    
    private bool used = false;
    private GameObject portalToCreate;

    private void OnCollisionEnter(Collision collision)
    {        
        // check for collisions with objects of default or block layer
        // comparetag needed for locked doors atm
        if ((collision.gameObject.layer == 0) && !collision.gameObject.CompareTag("Doorway") && !used)
        {
            Debug.Log("Collided with wall");

            GameObject portalParent = isNextPortalA ? portalA : portalB;

            Vector2 v2Direction = gameObject.GetComponent<Projectile>().projectileDirection;

            Quaternion rotation = new Quaternion();
            Vector3 spawnPosition = collision.contacts[0].point;
            if (v2Direction.y != 0)
            {
                spawnPosition.x = Mathf.RoundToInt(spawnPosition.x); // align portal to wall on X axis
                if (v2Direction.y > 0) // if laser is moving upwards
                {
                    spawnPosition += new Vector3(0, 0.75f, 0);
                }
                else // if laser is moving downwards
                {
                    spawnPosition += new Vector3(0, -0.75f, 0);
                }
            }
            else
            {
                spawnPosition.y = Mathf.RoundToInt(spawnPosition.y); // align portal to wall on Y axis
                if (v2Direction.x > 0) // if laser is moving rightwards
                {
                    rotation = Quaternion.Euler(0, 0, -90); // Rotates the portal by 90 degrees when hitting L/R walls
                    spawnPosition += new Vector3(0.75f, 0, 0);
                }
                else // if laser is moving leftwards
                {
                    rotation = Quaternion.Euler(0, 0, 90); // Rotates the portal by 90 degrees when hitting L/R walls
                    spawnPosition += new Vector3(-0.75f, 0, 0);
                    
                }
            }
            
            GameObject newPortal = Instantiate(portalParent, spawnPosition, rotation);

            Portal newPortalScript = newPortal.GetComponent<Portal>();
            newPortalScript.offset = (gameObject.GetComponent<Projectile>().projectileDirection * -1).normalized;
            
            PortalManager.instance.RegisterPortal(newPortalScript, isNextPortalA); // inform PortalManager of portal

            isNextPortalA = !isNextPortalA;
            used = true;
        }
        // this is if we hit another layer (i.e. doorways)
        else if (!used)
        {
            portalGun.SetNumPortals(-1);
        }
    }
}
