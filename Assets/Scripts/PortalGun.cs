using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PortalGun : Weapons
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
        if (!IsOnCooldown)
        {
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            if (prefabToInstantiate != null)
            {
                Instantiate(prefabToInstantiate, position, rotation);
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
        animator.HandleBowAnimation();
    }
}
