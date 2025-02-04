using UnityEngine; 
using System.Collections;
public class Bomb : Weapons 
{
    public GameObject prefab { get; set; }
    private Animator anim;
    private RoomTransition.Direction direction;
    public bool isDrop = true;
    public BoxCollider triggerColliderToEnable;

    private Inventory inv;

    public void Setup(GameObject bombPrefab, float cooldown, Inventory _inv)
    {
        prefab = bombPrefab;
        Cooldown = cooldown;
        IsOnCooldown = false;
        inv = _inv;
    }

    public override void HandleAnimation(InputToAnimator animator)
    { 
        direction = animator.GetPlayerDirection();
        animator.HandleBowAnimation();
    }

    public override void Attack(Vector3 position, Quaternion rotation, RoomTransition.Direction direction)
    {
        if (!IsOnCooldown && inv.GetBombs() > 0)
        {
            Cooldown_Left = Time.time + Cooldown;
            switch (direction)
            {
                case RoomTransition.Direction.Up:
                    position += Vector3.up;
                    break;
                case RoomTransition.Direction.Down:
                    position += Vector3.down;
                    break;
                case RoomTransition.Direction.Left:
                    position += Vector3.left;
                    break;
                case RoomTransition.Direction.Right:
                    position += Vector3.right;
                    break;
            }
            GameObject bombInst = Instantiate(prefab, position, rotation);
            bombInst.GetComponent<Bomb>().isDrop = false;
            inv.AddBomb(-1);
            anim = bombInst.GetComponentInChildren<Animator>();
            StartCoroutine(TriggerExplosion(bombInst));
            
            IsOnCooldown = true;
        }
        else if (Time.time >= Cooldown_Left)
        {
            IsOnCooldown = false;
        }
    }

    private IEnumerator TriggerExplosion(GameObject bomb)
    {
        yield return new WaitForSeconds(1f);
        if (anim != null)
        {
            anim.SetTrigger("Explode");
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            while (!stateInfo.IsName("Explode"))
            {
                yield return null;
                stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            }
            
            bomb.GetComponent<Bomb>().triggerColliderToEnable.enabled = true;

            yield return new WaitForSeconds(stateInfo.length);

            Destroy(bomb);
        }
    }
}
