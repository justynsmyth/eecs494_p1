using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 dest;
    public GameObject teleportTo;
    public float portalCooldown = 0.5f;

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
            other.gameObject.transform.position = teleportTo.transform.position + teleportTo.GetComponent<Portal>().offset;
            Debug.Log(teleportTo.GetComponent<Portal>().offset);

            SetTimeOfTeleport();
            teleportTo.GetComponent<Portal>().SetTimeOfTeleport();
        }
    }

    public void SetTimeOfTeleport()
    {
        timeOfTeleport = Time.time;
    }
}
