using UnityEngine;
using System.Collections;

public class Sword : Weapons
{
    public void Setup(GameObject up, GameObject down, GameObject left, GameObject right, float cooldown)
    {
        Prefab_Up = up;
        Prefab_Down = down;
        Prefab_Left = left;
        Prefab_Right = right;
        CooldownDuration = cooldown;
        IsOnCooldown = false;
    }

    public override void HandleAnimation(InputToAnimator animator)
    {
       animator.HandleSwordAnimation(); 
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        if (GameManager.instance.player_health.HasMaxHealth() && !IsOnCooldown)
        {
            Cooldown_Left = Time.time + CooldownDuration;
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            Instantiate(prefabToInstantiate, position, rotation);
            IsOnCooldown = true;
            StartCoroutine(ResetCooldownCoroutine());
        }
    }
    
    protected IEnumerator ResetCooldownCoroutine()
    {
        yield return new WaitForSeconds(CooldownDuration);
        IsOnCooldown = false;
    }
}


