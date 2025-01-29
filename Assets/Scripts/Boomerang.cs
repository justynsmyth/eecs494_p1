using UnityEngine;

public class Boomerang : Weapons
{
    public GameObject prefab { get; set; }
    public bool isDrop = true;
    private GameObject _owner;
    public void Setup(GameObject boomPrefab, float cooldown, GameObject owner)
    {
        prefab = boomPrefab; 
        Cooldown = cooldown;
        IsOnCooldown = false;
        _owner = owner;
    }

    public override void HandleAnimation(InputToAnimator animator)
    {
        animator.HandleBowAnimation();
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        if (!IsOnCooldown)
        {
            Cooldown_Left = Time.time + Cooldown;
            GameObject b = Instantiate(prefab, position, rotation);
            b.GetComponent<Boomerang>().isDrop = false;
            BoomerangProjectile projectile = b.GetComponent<BoomerangProjectile>();

            if (projectile != null)
            {
                projectile.Setup(_owner);
                projectile.OnReturn += HandleReturn;
            }

            IsOnCooldown = true;
        }
    }

    private void HandleReturn()
    {
        IsOnCooldown = false;
    }
}
