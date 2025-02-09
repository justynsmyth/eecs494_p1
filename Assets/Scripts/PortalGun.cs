using System.Collections;
using UnityEngine;

public class PortalGun : Weapons
{
    private Inventory inv;
    private int numPortals = 0;
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
        Portal portalA = PortalManager.instance.GetPortalA();
        Portal portalB = PortalManager.instance.GetPortalB();
        if (portalA != null && portalB != null)
        {
            PortalManager.instance.DestroyPortals();
            numPortals = 0;
            return;
        }
        if (!IsOnCooldown && numPortals < 2)
        {
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            if (prefabToInstantiate != null)
            {
                GameObject projectileLaser = Instantiate(prefabToInstantiate, position, rotation);
                projectileLaser.GetComponent<PortalLaser>().portalGun = this;

                IsOnCooldown = true;
                SetNumPortals(1);   

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

    public void SetNumPortals(int num)
    {
        numPortals += num;
    }
}
