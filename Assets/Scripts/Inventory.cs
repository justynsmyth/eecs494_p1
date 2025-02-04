using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;
    int key_count = 0;
    int bomb_count = 0;

    int max_inventory_value = 999;

    public GameObject swordProjectilePrefab_Left;
    public GameObject swordProjectilePrefab_Right;
    public GameObject swordProjectilePrefab_Up;
    public GameObject swordProjectilePrefab_Down;
    
    public GameObject ArrowProjectilePrefab_Left;
    public GameObject ArrowProjectilePrefab_Right;
    public GameObject ArrowProjectilePrefab_Up;
    public GameObject ArrowProjectilePrefab_Down;

    public GameObject PortalGunProjectilePrefab_Left;
    public GameObject PortalGunProjectilePrefab_Right;
    public GameObject PortalGunProjectilePrefab_Up;
    public GameObject PortalGunProjectilePrefab_Down;

    public GameObject BoomerangPrefab;

    public GameObject BombPrefab;

    private InputToAnimator playerAnimator;
    private Rigidbody rb;

    public float ProjectileCooldown = 1f;

    public GameObject WeaponPrefab;

    public AudioClip swordSound;
    public AudioClip swordFullHPSound;
    public AudioClip bowSound;
    public AudioClip boomerangSound;
    public AudioClip bombSound;
    public AudioClip laserSound;

    private Dictionary<string, Weapons> weapons;
    
    
    // When adding Weapons, Instantiate their component here
    // Then add them to the weapons dictionary
    void Start()
    {
        playerAnimator = GetComponent<InputToAnimator>();
        Sword sword = Instantiate(WeaponPrefab).GetComponent<Sword>();
        Bow bow = Instantiate(WeaponPrefab).GetComponent<Bow>();
        Boomerang boomerang = Instantiate(WeaponPrefab).GetComponent<Boomerang>();
        Bomb bomb = Instantiate(WeaponPrefab).GetComponent<Bomb>();
        PortalGun portal_gun = Instantiate(WeaponPrefab).GetComponent<PortalGun>();

        sword.Setup(swordProjectilePrefab_Up, swordProjectilePrefab_Down, swordProjectilePrefab_Left, swordProjectilePrefab_Right, ProjectileCooldown);
        bow.Setup(ArrowProjectilePrefab_Up, ArrowProjectilePrefab_Down, ArrowProjectilePrefab_Left, ArrowProjectilePrefab_Right, ProjectileCooldown, this);
        boomerang.Setup(BoomerangPrefab, ProjectileCooldown * 1.5f, gameObject); // boomerang needs longer to cooldown
        bomb.Setup(BombPrefab, ProjectileCooldown, this);
        portal_gun.Setup(PortalGunProjectilePrefab_Up, PortalGunProjectilePrefab_Down, PortalGunProjectilePrefab_Left, PortalGunProjectilePrefab_Right, ProjectileCooldown, this);

        weapons = new Dictionary<string, Weapons>
        {
            { "Sword", sword },
            { "Bow",   bow },
            { "Bomb", bomb},
            { "Boomerang", boomerang },
            { "Portal Gun", portal_gun }
        }; 
    }

    public void AddRupees(int num_rupees)
    {
        if (!GameManager.god_mode && rupee_count + num_rupees <= max_inventory_value)
        {
            rupee_count += num_rupees;
        }
    }

    public void AddKeys(int num_keys)
    {
        if (!GameManager.god_mode && key_count + num_keys <= max_inventory_value)
        {
            key_count += num_keys;
        }
    }

    public void AddBomb(int num_bombs)
    {
        if (!GameManager.god_mode && bomb_count + num_bombs <= max_inventory_value)
        {
            bomb_count += num_bombs;
        }
    }

    public void MaximizeResources()
    {
        rupee_count = max_inventory_value;
        key_count = max_inventory_value;
        bomb_count = max_inventory_value;
    }

    public void UnlockAllWeapons()
    {
        foreach (var key in zSlotItems.Keys.ToList())
        {
            zSlotItems[key] = true;
        }
    }

    public int GetRupees()
    {
        return rupee_count;
    }

    public int GetKeys()
    {
        return key_count;
    }

    public int GetBombs()
    {
        return bomb_count;
    }

    // Used to identify the Z Slot. Update this to include more Z slot items
    // All items are set to false.
    private string currentZSlotItem = "";
    private readonly Dictionary<string, bool> zSlotItems = new Dictionary<string, bool>
    {
        { "Bow", false },
        { "Bomb", false },
        { "Boomerang", false },
        { "Portal Gun", false }
    };
    
    private string currentXSlotItem = "Sword";
    private readonly HashSet<string> xSlotItems = new HashSet<string> { "Sword" };
    
    public void UpdateZSlotItem(string newItem)
    {
        if (!zSlotItems.ContainsKey(newItem)) return;
        zSlotItems[newItem] = true; // unlock the weapon
        currentZSlotItem = newItem;
        Debug.Log($"Z-slot updated to: {currentZSlotItem}");
    }


    public void UpdateXSlotItem(string newItem)
    {
        if (!xSlotItems.Contains(newItem)) return;
        currentXSlotItem = newItem;
        Debug.Log($"X-slot updated to: {currentXSlotItem}");
    }
    public string GetCurrentZSlotItem() { return currentZSlotItem; }
    public string GetCurrentXSlotItem() { return currentXSlotItem; }
    
    private Coroutine currentCoroutine;
    
    void OnEnable()
    {
        PlayerInput.OnXPressed += () => UseWeapon(currentXSlotItem);
        PlayerInput.OnZPressed += () => UseWeapon(currentZSlotItem);
        PlayerInput.OnSpacePressed += SwapZSlotItem; 
    }

    void OnDisable()
    {
        PlayerInput.OnXPressed -= () => UseWeapon(currentXSlotItem);
        PlayerInput.OnZPressed -= () => UseWeapon(currentZSlotItem);
        PlayerInput.OnSpacePressed -= SwapZSlotItem;
    }
    private void SwapZSlotItem()
    {
        // Get only unlocked items
        // 1. Filter values that are true 2. select the keys for each of these 3. convert it to a new list
        var unlockedItems = zSlotItems.Where(item => item.Value)
            .Select(item => item.Key)
            .ToList();
        if (unlockedItems.Count == 0)
        {
            Debug.Log("No unlocked Z-slot items to swap to.");
            return;
        }

        int currentIndex = unlockedItems.IndexOf(currentZSlotItem);
        int nextIndex = (currentIndex + 1) % unlockedItems.Count;
        currentZSlotItem = unlockedItems[nextIndex];

        Debug.Log($"Z-slot item swapped to: {currentZSlotItem}");
    }

    
    private void UseWeapon(string weaponName)
    {
        if (weaponName == "") return;
        if (currentCoroutine != null) return;
        currentCoroutine = StartCoroutine(HandleAttack(weaponName));
    }
    
    private IEnumerator HandleAttack(string weaponName)
    {
        PlayerInput.IsActionInProgress = true;
        weapons[weaponName].HandleAnimation(playerAnimator);

        RoomTransition.Direction direction = playerAnimator.GetPlayerDirection();
        weapons[weaponName].Attack(transform.position, Quaternion.identity, direction);
        
        // TODO: bugfix this later, rough implementation
        if (weaponName == "Sword")
        {
            AudioSource.PlayClipAtPoint(swordSound, Camera.main.transform.position);
            if (GetComponent<HasHealth>().GetHealth() == GetComponent<HasHealth>().GetMaxHealth())
            {
                AudioSource.PlayClipAtPoint(swordFullHPSound, Camera.main.transform.position);
            }
        }
        else if (weaponName == "Bow")
        {
            AudioSource.PlayClipAtPoint(bowSound, Camera.main.transform.position);
        }
        else if (weaponName == "Boomerang")
        {
            AudioSource.PlayClipAtPoint(boomerangSound, Camera.main.transform.position);
        }
        else if (weaponName == "Bomb")
        {
            AudioSource.PlayClipAtPoint(bombSound, Camera.main.transform.position);
        }
        else if (weaponName == "Portal Gun")
        {
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position);
        }

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);
        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }
}