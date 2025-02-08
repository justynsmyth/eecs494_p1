using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 dest;
    public GameObject teleportTo;
    public float portalCooldown = 0.5f;
    public float offsetMagnitude = 2f;

    private string direction;
    private float timeOfTeleport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "PortalB(Clone)")
        {
            teleportTo = GameObject.Find("PortalA(Clone)");
        }
        else if (gameObject.name == "PortalA(Clone)")
        {
            teleportTo = GameObject.Find("PortalB(Clone)");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if a 2nd portal exists, then teleport player to that portal
        if (teleportTo && (Time.time - timeOfTeleport >= portalCooldown))
        {
            Portal otherPortal = teleportTo.GetComponent<Portal>();

            // weapon projectiles should hit portal and come out on the correct direction
            if (other.gameObject.CompareTag("Weapon"))
            {
                // rotate the projectile (based on difference in projectile direction and offset direction)
                // we rotate in intervals of 90 degrees
                Projectile projectile = other.gameObject.GetComponent<Projectile>();
                int projectileDirection = GetRotationValue(projectile.projectileDirection);
                Vector2 offset2D = otherPortal.offset;
                int directionDifference = projectileDirection - GetRotationValue(offset2D);
                int timesToRotate = (4 + directionDifference) % 4;
                other.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90 * timesToRotate);

                // change the projectile's direction (based on offset direction)
                projectile.projectileDirection = teleportTo.GetComponent<Portal>().offset;

                // change the projectile's linearVelocity based on new direction
                projectile.UpdateMovement();
            }
            // TODO: implement boomerang interaction with portal for Goriyas and player boomerangs
            else if (1 == 0)
            {
                // do stuff
            }
            // this is for Player/Enemy gameObjects
            else
            {
            
            }

            other.gameObject.transform.position = teleportTo.transform.position + (teleportTo.GetComponent<Portal>().offset * offsetMagnitude);
            SetTimeOfTeleport();
            teleportTo.GetComponent<Portal>().SetTimeOfTeleport();
        }
    }

    private int GetRotationValue(Vector2 direction)
    {
        // right = 0
        // down = 1
        // left = 2
        // up = 3
        if (direction.x == 1)
        {
            return 0;
        }
        else if (direction.y == -1)
        {
            return 1;
        }
        else if (direction.x == -1)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    public void SetTimeOfTeleport()
    {
        timeOfTeleport = Time.time;
    }
}
