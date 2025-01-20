using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;
    
    public GameObject swordProjectilePrefab_Left;
    public GameObject swordProjectilePrefab_Right;
    public GameObject swordProjectilePrefab_Up;
    public GameObject swordProjectilePrefab_Down;
    
    public GameObject ArrowProjectilePrefab_Left;
    public GameObject ArrowProjectilePrefab_Right;
    public GameObject ArrowProjectilePrefab_Up;
    public GameObject ArrowProjectilePrefab_Down;

    private InputToAnimator playerAnimator;
    private Rigidbody rb;

    public float ProjectileCooldown = 1f;

    public GameObject WeaponPrefab;

    private Dictionary<string, Weapons> weapons;
    void Start()
    {
        playerAnimator = GetComponent<InputToAnimator>();
        Sword sword = Instantiate(WeaponPrefab).GetComponent<Sword>();
        Bow bow = Instantiate(WeaponPrefab).GetComponent<Bow>();

        sword.Setup(swordProjectilePrefab_Up, swordProjectilePrefab_Down, swordProjectilePrefab_Left, swordProjectilePrefab_Right, 1f);
        bow.Setup(ArrowProjectilePrefab_Up, ArrowProjectilePrefab_Down, ArrowProjectilePrefab_Left, ArrowProjectilePrefab_Right, 1f, this);
        
        weapons = new Dictionary<string, Weapons>
        {
            { "Sword", sword },
            { "Bow",   bow }
        }; 
    }

    public void AddRupees(int num_rupees)
    {
        if (!GameManager.god_mode)
        {
            rupee_count += num_rupees;
        }
    }

    public void MaximizeResources(int num_rupees)
    {
        rupee_count = num_rupees;
    }

    public int GetRupees()
    {
        return rupee_count;
    }

    public static bool HasSword = true;
    
    private Coroutine currentCoroutine;
    
    void OnEnable()
    {
        PlayerInput.OnXPressed += () => UseWeapon("Sword");
        PlayerInput.OnZPressed += () => UseWeapon("Bow");
    }

    void OnDisable()
    {
        PlayerInput.OnXPressed -= () => UseWeapon("Sword");
        PlayerInput.OnZPressed -=  () => UseWeapon("Bow");
    }
    
    private void UseWeapon(string weaponName)
    {
        if (currentCoroutine != null) return;
        currentCoroutine = StartCoroutine(HandleAttack(weaponName));
    }
    
    private IEnumerator HandleAttack(string weaponName)
    {
        PlayerInput.IsActionInProgress = true;
        weapons[weaponName].HandleAnimation(playerAnimator);

        RoomTransition.Direction direction = playerAnimator.GetPlayerDirection();
        Debug.Log(direction);
        weapons[weaponName].Attack(transform.position, Quaternion.identity, direction);

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);
        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }
}