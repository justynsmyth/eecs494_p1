using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 dest;

    [SerializeField]
    private GameObject teleportTo; // PortalManager will set the value for us 
    public float portalCooldown = 0.5f;
    public float offsetMagnitude = 1.5f;
    
    private float timeOfTeleport;
    
    public GameObject portalPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // check if a 2nd portal exists, then teleport player to that portal
        if (teleportTo != null && (Time.time - timeOfTeleport >= portalCooldown))
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

            Vector3 newPos = teleportTo.transform.position +
                             (teleportTo.GetComponent<Portal>().offset * offsetMagnitude);

            Debug.Log(offsetMagnitude);

            if (teleportTo.GetComponent<Portal>().offset.y != 0) // align after teleporting to avoid clipping
            {
                newPos.y = Mathf.RoundToInt(newPos.y);
            }
            else // 
            {
                newPos.x = Mathf.RoundToInt(newPos.x);
            }
            
            other.gameObject.transform.position = newPos;
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

    public void SetTeleportTo(GameObject other)
    {
        teleportTo = other;
    }
}
