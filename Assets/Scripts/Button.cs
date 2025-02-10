using System;
using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{

    public UnlockDoor door;
    public bool isANDType = false;
    public bool isProjectileType = false;
    public GameObject otherButtonTile; // used if isANDTypeq
    private Button otherButton;

    public float closeDelay = 0.5f;

    private bool isTriggered = false;
    void Start()
    {
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
                    door.DoorUnlock();
                }
            }
            else
            {
                door.DoorUnlock();
            }
        } if (other.CompareTag("Weapon") | other.CompareTag("boomerang"))
        {
            if (!isProjectileType) return;
            isTriggered = true;
            Debug.Log(gameObject.name + " is currently triggered");

            if (isANDType)
            {
                if (otherButton != null && otherButton.isTriggered)
                {
                    door.DoorUnlock();
                }
            }
            else if (door != null)
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
