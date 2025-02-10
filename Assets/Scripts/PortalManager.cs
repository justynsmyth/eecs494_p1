using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance { get; private set; }

    private Portal portalA;
    private Portal portalB;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPortal(Portal newPortal, bool isPortalA)
    {
        if (isPortalA)
            portalA = newPortal;
        else
            portalB = newPortal;

        if (portalA != null && portalB != null)
        {
            portalA.SetTeleportTo(portalB.gameObject);
            portalB.SetTeleportTo(portalA.gameObject);
        }
    }
    
    public Portal GetPortalA() => portalA;
    public Portal GetPortalB() => portalB;
    public void DestroyPortals()
    {
        if (portalA != null)
        {
            portalA.SetTeleportTo(null);
            Destroy(portalA.gameObject);
            portalA = null;
        }
        if (portalB != null)
        {
            portalB.SetTeleportTo(null);
            Destroy(portalB.gameObject);
            portalB = null;
        }
        GameManager.instance.inventory.ResetPortals();
    }
}
