using UnityEngine;

public class Boomerang : Weapons
{
    public GameObject prefab { get; set; }
    public bool isDrop = true;
    private GameObject _owner;
    public void Setup(GameObject boomPrefab, float cooldown, AudioClip sound, GameObject owner)
    {
        prefab = boomPrefab; 
        CooldownDuration = cooldown;
        IsOnCooldown = false;
        SoundClip = sound;
        _owner = owner;
    }

    public override void HandleAnimation(InputToAnimator animator)
    {
        if (!IsOnCooldown)
        {
            animator.HandleBowAnimation();
        }
        else
        {
            PlayerInput.IsActionInProgress = false;
        }
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        if (!IsOnCooldown)
        {
            Cooldown_Left = Time.time + CooldownDuration;
            GameObject b = Instantiate(prefab, position, rotation);
            Boomerang newBoomerang = b.GetComponent<Boomerang>();
            newBoomerang.isDrop = false;
            AudioSource.PlayClipAtPoint(SoundClip, Camera.main.transform.position);
            newBoomerang.Setup(prefab, CooldownDuration, SoundClip, _owner); // Ensure the owner is set for each boomerang
            
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
