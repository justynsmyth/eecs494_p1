using UnityEngine;
using System.Collections;

public class Sword : Weapons
{
    public void Setup(GameObject up, GameObject down, GameObject left, GameObject right, float cooldown, AudioClip sound, AudioClip fullHPSound)
    {
        Prefab_Up = up;
        Prefab_Down = down;
        Prefab_Left = left;
        Prefab_Right = right;
        CooldownDuration = cooldown;
        SoundClip = sound;
        SoundClip2 = fullHPSound;
        IsOnCooldown = false;
    }

    public override void HandleAnimation(InputToAnimator animator)
    {
       animator.HandleSwordAnimation(); 
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        AudioSource.PlayClipAtPoint(SoundClip, Camera.main.transform.position);

        if (GameManager.instance.player_health.HasMaxHealth() && !IsOnCooldown)
        {
            Cooldown_Left = Time.time + CooldownDuration;
            GameObject prefabToInstantiate = SelectPrefabByDirection(direction);
            Instantiate(prefabToInstantiate, position, rotation);
            AudioSource.PlayClipAtPoint(SoundClip2, Camera.main.transform.position);
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


