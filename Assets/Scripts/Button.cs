using System;
using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{

    public UnlockDoor door;
    public bool isANDType = false;
    public bool isProjectileType = false;
    public GameObject otherButtonTile; // used if isANDType
    private Button otherButton;

    public GameObject otherButtonTile2;
    private Button otherButton2;

    public float closeDelay = 0.5f;

    private bool isTriggered = false;
    void Start()
    {
        if (door == null) Debug.LogError("door is null. Set in inspector!");
        if (isANDType)
        {
            isProjectileType = false; // Just in case both are turned on
            if (otherButtonTile == null)
            {
                Debug.LogError("otherButton is null. Set in inspector!");
            }
            else
            {
                otherButton = otherButtonTile.GetComponent<Button>();
                if (otherButton2 != null)
                {
                    otherButton2 = otherButtonTile2.GetComponent<Button>();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CompanionBlock"))
        {
            if (isProjectileType) return;
            isTriggered = true;
            if (isANDType)
            {
                if (otherButton != null && otherButton.isTriggered)
                {
                    if (otherButton2 != null)
                    {
                        if (otherButton2.isTriggered)
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
            else
            {
                door.DoorUnlock();
            }
        } else if (other.CompareTag("Weapon") | other.CompareTag("boomerang"))
        {
            if (!isProjectileType) return;
            isTriggered = true;

            if (door != null)
            {
                door.DoorUnlock();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("CompanionBlock"))
        {
            if (isProjectileType) return;
            StartCoroutine(DelayedDoorLock());
        } else if (other.CompareTag("Weapon") || other.CompareTag("boomerang"))
        {
            if (!isProjectileType) return;
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
