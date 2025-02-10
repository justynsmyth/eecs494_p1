using System;
using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{

    public UnlockDoor door;
    public bool isANDType = false;
    public GameObject otherButtonTile; // used if isANDType
    private Button otherButton;
    public float closeDelay = 0.5f;

    private bool isTriggered = false;
    void Start()
    {
        if (door == null) Debug.LogError("door is null. Set in inspector!");
        if (isANDType)
        {
            if (otherButtonTile == null)
            {
                Debug.LogError("otherButton is null. Set in inspector!");
            }
            else
            {
                otherButton = otherButtonTile.GetComponent<Button>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CompanionBlock"))
        {
            isTriggered = true;
            if (isANDType)
            {
                if (otherButton != null && otherButton.isTriggered)
                {
                    door.DoorUnlock();
                }
            }
            else
            {
                door.DoorUnlock();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CompanionBlock"))
        {
            StartCoroutine(DelayedDoorLock());
        }
    }
    
    IEnumerator DelayedDoorLock()
    {
        yield return new WaitForSeconds(closeDelay);
        isTriggered = false;
        door.DoorLock();
    }
}
