using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;

    private InputToAnimator playerAnimator;
    private Rigidbody rb;


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
    }

    void OnDisable()
    {
        PlayerInput.OnXPressed -= AttackSword;
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
        currentCoroutine = StartCoroutine(HandleAttack());
    }

    private IEnumerator HandleAttack()
    {
        PlayerInput.IsActionInProgress = true;

        playerAnimator.HandleSwordAnimation();

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);

        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }
}