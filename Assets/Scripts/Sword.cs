using UnityEngine;

public class Sword : Weapons
{
    public void Setup(GameObject up, GameObject down, GameObject left, GameObject right, float cooldown)
    {
        Prefab_Up = up;
        Prefab_Down = down;
        Prefab_Left = left;
        Prefab_Right = right;
        Cooldown = cooldown;
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
            Cooldown_Left = Time.time + Cooldown;
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            Instantiate(prefabToInstantiate, position, rotation);
            IsOnCooldown = true;
        }
        else if (Time.time >= Cooldown_Left)
        {
           IsOnCooldown = false; 
        }
    }
}


