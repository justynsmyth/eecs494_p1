using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;

    private InputToAnimator playerAnimator;
    private Rigidbody rb;

    public GameObject swordProjectilePrefab_Left;
    public GameObject swordProjectilePrefab_Right;
    public GameObject swordProjectilePrefab_Up;
    public GameObject swordProjectilePrefab_Down;
    
    public GameObject ArrowProjectilePrefab_Left;
    public GameObject ArrowProjectilePrefab_Right;
    public GameObject ArrowProjectilePrefab_Up;
    public GameObject ArrowProjectilePrefab_Down;

    public float ProjectileCooldown = 1f;
    private bool onCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        playerAnimator = GetComponent<InputToAnimator>();
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

    void OnEnable()
    {
        PlayerInput.OnXPressed += AttackSword;
        PlayerInput.OnZPressed += AttackBow;
    }

    void OnDisable()
    {
        PlayerInput.OnXPressed -= AttackSword;
        PlayerInput.OnZPressed -= AttackBow;
    }

    private Coroutine currentCoroutine;
    
    /**
     * TODO: Need to Enable / Disable Sword. Need to move into Weapons.cs file for Bow and Sword Polymorphic purposes.
     * ! Sword is always attached to X Button for now.
     */
    private void AttackSword()
    {
        if (currentCoroutine != null)
        {
            return;
        }
        currentCoroutine = StartCoroutine(HandleSwordAttack());
    }

    private IEnumerator HandleSwordAttack()
    {
        PlayerInput.IsActionInProgress = true;

        playerAnimator.HandleSwordAnimation();

        if (GameManager.instance.player_health.HasMaxHealth() && !onCooldown)
        {
            onCooldown = true;
            cooldownTimer = Time.time + ProjectileCooldown;
            // Spawn a projectile sword
            RoomTransition.Direction d = playerAnimator.GetPlayerDirection();
            switch (d)
            {
                case RoomTransition.Direction.Up:
                    Instantiate(swordProjectilePrefab_Up, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Down:
                    Instantiate(swordProjectilePrefab_Down, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Left:
                    Instantiate(swordProjectilePrefab_Left, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Right:
                    Instantiate(swordProjectilePrefab_Right, transform.position, Quaternion.identity);
                    break;
            }
        }
        if (Time.time >= cooldownTimer)
        {
            onCooldown = false;
        }

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);

        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }

    private void AttackBow()
    {
        if (currentCoroutine != null)
        {
            return;
        }
        currentCoroutine = StartCoroutine(HandleBowAttack());
    }

    private IEnumerator HandleBowAttack()
    {
        PlayerInput.IsActionInProgress = true;

        playerAnimator.HandleBowAnimation();

        if (GetRupees() > 0)
        {
            onCooldown = true;
            cooldownTimer = Time.time + ProjectileCooldown;
            // Spawn a Arrow 
            RoomTransition.Direction d = playerAnimator.GetPlayerDirection();
            switch (d)
            {
                case RoomTransition.Direction.Up:
                    Instantiate(ArrowProjectilePrefab_Up, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Down:
                    Instantiate(ArrowProjectilePrefab_Down, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Left:
                    Instantiate(ArrowProjectilePrefab_Left, transform.position, Quaternion.identity);
                    break;
                case RoomTransition.Direction.Right:
                    Instantiate(ArrowProjectilePrefab_Right, transform.position, Quaternion.identity);
                    break;
            }
            // Remove Rupee in exchange for Arrow 
            AddRupees(-1);
        }

        if (Time.time >= cooldownTimer)
        {
            onCooldown = false;
        }

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);

        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }
}