using UnityEngine;
using System.Collections;

public class InputToAnimator : MonoBehaviour
{
    public Sprite downIdle;
    public Sprite leftIdle;
    public Sprite rightIdle;
    public Sprite upIdle;

    public Sprite downMoving;
    public Sprite leftMoving;
    public Sprite rightMoving;
    public Sprite upMoving;

    public Sprite downAttack;
    public Sprite leftAttack;
    public Sprite rightAttack;
    public Sprite upAttack;

    public float moveRate = 0.15f;
    public float attackAnimationDuration = 0.25f;

    private SpriteRenderer currentSprite;
    private float timeSinceAnimating;

    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (RoomTransition.IsTransitionInProgress() || PlayerInput.IsActionInProgress)
        {
            return; 
        }
        
        timeSinceAnimating += Time.deltaTime;

        HandleMovementAnimation();
    }

    private void HandleMovementAnimation()
    {
        // Take horizontal movement inputs (A, D, <-, ->)
        float horizontal_input = Input.GetAxisRaw("Horizontal");

        // Take vertical movement inputs (W, S, up, down)
        float vertical_input = Input.GetAxisRaw("Vertical");

        // Update the sprite here based on movement
        if (horizontal_input > 0.0f)
        {
            ChangeSprite(rightMoving, rightIdle);
        }
        else if (horizontal_input < 0.0f)
        {
            ChangeSprite(leftMoving, leftIdle);
        }
        else if (vertical_input > 0.0f)
        {
            ChangeSprite(upMoving, upIdle);
        }
        else if (vertical_input < 0.0f)
        {
            ChangeSprite(downMoving, downIdle);
        }
    }

    private void ChangeSprite(Sprite movingSprite, Sprite idleSprite)
    {
        if (currentSprite.sprite == movingSprite && timeSinceAnimating >= moveRate)
        {
            currentSprite.sprite = idleSprite;
            timeSinceAnimating = 0.0f;
        }
        else if (currentSprite.sprite == idleSprite && timeSinceAnimating >= moveRate)
        {
            currentSprite.sprite = movingSprite;
            timeSinceAnimating = 0.0f;
        }
        else if (currentSprite.sprite != idleSprite && currentSprite.sprite != movingSprite)
        {
            currentSprite.sprite = movingSprite;
        }
    }

    public void HandleAttackAnimation()
    {
        Sprite previousSprite = currentSprite.sprite;
        
        currentSprite.sprite = GetPlayerDirection() switch
        {
            RoomTransition.Direction.Up => upAttack,
            RoomTransition.Direction.Down => downAttack,
            RoomTransition.Direction.Left => leftAttack,
            RoomTransition.Direction.Right => rightAttack,
            _ => rightAttack
        };

        StartCoroutine(ResetSpriteAfterAttack(previousSprite));
    }

    private IEnumerator ResetSpriteAfterAttack(Sprite previousSprite)
    {
        yield return new WaitForSeconds(attackAnimationDuration);

        currentSprite.sprite = previousSprite;
        
    }

    public RoomTransition.Direction GetPlayerDirection()
    {
        return currentSprite.sprite switch
        {
            var s when s == rightMoving || s == rightIdle => RoomTransition.Direction.Right, 
            var s when s == leftMoving || s == leftIdle => RoomTransition.Direction.Left, 
            var s when s == upMoving || s == upIdle => RoomTransition.Direction.Up, 
            var s when s == downMoving || s == downIdle => RoomTransition.Direction.Down,
            _ => RoomTransition.Direction.Right 
        };
    }
}
