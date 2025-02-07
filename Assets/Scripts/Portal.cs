using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 dest;
    public GameObject teleportTo;

    private string direction;

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
        if (teleportTo)
        {
            other.gameObject.transform.position = teleportTo.transform.position + teleportTo.GetComponent<Portal>().offset;
        }
    }
}
