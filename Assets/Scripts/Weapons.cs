using System;
using UnityEngine;
using System.Collections;

public abstract class Weapons : MonoBehaviour
{
    public GameObject Prefab_Up { get; set; }
    public GameObject Prefab_Down { get; set; }
    public GameObject Prefab_Left { get; set; }
    public GameObject Prefab_Right { get; set; }

    public float Cooldown{ get; set; }
    public bool IsOnCooldown { get; set; }
    
    public float Cooldown_Left { get; set; }

    public abstract void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction);

    public abstract void HandleAnimation(InputToAnimator animator);
    protected GameObject SelectPrefabByDirection(RoomTransition.Direction direction)
    {
        switch (direction)
        {
            case RoomTransition.Direction.Up:
                return Prefab_Up;
            case RoomTransition.Direction.Down:
                return Prefab_Down;
            case RoomTransition.Direction.Left:
                return Prefab_Left;
            case RoomTransition.Direction.Right:
                return Prefab_Right;
            default:
                return null;
        }
    }
}

