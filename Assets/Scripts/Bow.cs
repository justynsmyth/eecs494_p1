using UnityEngine;
using System.Collections;

public class Bow : Weapons
{ 
    private Inventory inv;
    public void Setup(GameObject up, GameObject down, GameObject left, GameObject right, float cooldown, Inventory _inv)
    {
        Prefab_Up = up;
        Prefab_Down = down;
        Prefab_Left = left;
        Prefab_Right = right;
        CooldownDuration = cooldown;
        IsOnCooldown = false;
        inv = _inv;
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        if (!IsOnCooldown && inv.GetRupees() > 0)
        {
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            if (prefabToInstantiate != null)
            {
                Instantiate(prefabToInstantiate, position, rotation);
                inv.AddRupees(-1);
                IsOnCooldown = true;
                inv.StartCoroutine(ResetCooldownCoroutine());
            }
        }
    }
    
    protected IEnumerator ResetCooldownCoroutine()
    {
        yield return new WaitForSeconds(CooldownDuration);
        IsOnCooldown = false;
    }

    public override void HandleAnimation(InputToAnimator animator)
    {
        if (inv.GetRupees() > 0)
        {
            animator.HandleBowAnimation();
        }
    }
}