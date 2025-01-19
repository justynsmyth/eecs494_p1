using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;

    private InputToAnimator playerAnimator;
    private Rigidbody rb;

    public BoxCollider swordCol;

    void Start()
    {
        playerAnimator = GetComponent<InputToAnimator>();

        if (swordCol != null)
        {
            swordCol.enabled = false;
        }
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
    private void AttackSword()
    {
        if (currentCoroutine != null)
        {
            return;
        }
        currentCoroutine = StartCoroutine(HandleAttackAnimation());
    }

    private IEnumerator HandleAttackAnimation()
    {
        PlayerInput.IsActionInProgress = true;

        if (swordCol != null)
        {
            swordCol.enabled = true;
        }

        playerAnimator.HandleAttackAnimation();

        yield return new WaitForSeconds(playerAnimator.attackAnimationDuration);

        if (swordCol != null)
        {
            swordCol.enabled = false;
        }

        PlayerInput.IsActionInProgress = false;
        currentCoroutine = null;
    }
}