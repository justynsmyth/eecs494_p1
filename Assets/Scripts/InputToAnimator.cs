using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

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

    public Sprite upBowAttack;
    public Sprite downBowAttack;
    public Sprite leftBowAttack;
    public Sprite rightBowAttack;
    
    public Sprite upPortalAttack;
    public Sprite downPortalAttack;
    public Sprite leftPortalAttack;
    public Sprite rightPortalAttack;

    public float moveRate = 0.15f;
    public float attackAnimationDuration = 0.25f;

    // Used to enable/disable the sword's trigger boxes when a sword animation is triggered.
    public BoxCollider leftSwordCollider;
    public BoxCollider rightSwordCollider;
    public BoxCollider upSwordCollider;
    public BoxCollider downSwordCollider;

    private SpriteRenderer currentSprite;
    private float timeSinceAnimating;

    private BoxCollider playerCollider;
    private bool movementEnabled = true;
    
    private void Awake()
    {
        spriteDirectionMap = new Dictionary<Sprite, RoomTransition.Direction>
        {
            // Right direction sprites
            { rightMoving, RoomTransition.Direction.Right },
            { rightIdle, RoomTransition.Direction.Right },
            { rightAttack, RoomTransition.Direction.Right },
            { rightBowAttack, RoomTransition.Direction.Right },
            { rightPortalAttack, RoomTransition.Direction.Right},

            // Left direction sprites
            { leftMoving, RoomTransition.Direction.Left },
            { leftIdle, RoomTransition.Direction.Left },
            { leftAttack, RoomTransition.Direction.Left },
            { leftBowAttack, RoomTransition.Direction.Left },
            { leftPortalAttack, RoomTransition.Direction.Left},

            // Up direction sprites
            { upMoving, RoomTransition.Direction.Up },
            { upIdle, RoomTransition.Direction.Up },
            { upAttack, RoomTransition.Direction.Up },
            { upBowAttack, RoomTransition.Direction.Up },
            { upPortalAttack, RoomTransition.Direction.Up},

            // Down direction sprites
            { downMoving, RoomTransition.Direction.Down },
            { downIdle, RoomTransition.Direction.Down },
            { downAttack, RoomTransition.Direction.Down },
            { downBowAttack, RoomTransition.Direction.Down },
            { downPortalAttack, RoomTransition.Direction.Down },
        };
    }

    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (RoomTransition.IsTransitionInProgress() || PlayerInput.IsActionInProgress || !movementEnabled)
        {
            return;
        }

        timeSinceAnimating += Time.deltaTime;

        HandleMovementAnimation();
    }

    public void ToggleMovement()
    {
        movementEnabled = !movementEnabled;
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

    public void HandleSwordAnimation()
    {
        Sprite previousSprite = currentSprite.sprite;

        RoomTransition.Direction direction = GetPlayerDirection();
        EnableSwordCollider(direction);

        currentSprite.sprite = direction switch
        {
            RoomTransition.Direction.Up => upAttack,
            RoomTransition.Direction.Down => downAttack,
            RoomTransition.Direction.Left => leftAttack,
            RoomTransition.Direction.Right => rightAttack,
            _ => rightAttack
        };

        StartCoroutine(ResetSpriteAfterAttack(previousSprite));
    }
    
    public void HandleBowAnimation()
    {
        Sprite previousSprite = currentSprite.sprite;

        RoomTransition.Direction direction = GetPlayerDirection();

        currentSprite.sprite = direction switch
        {
            RoomTransition.Direction.Up => upBowAttack,
            RoomTransition.Direction.Down => downBowAttack,
            RoomTransition.Direction.Left => leftBowAttack,
            RoomTransition.Direction.Right => rightBowAttack,
            _ => rightBowAttack
        };

        StartCoroutine(ResetSpriteAfterAttack(previousSprite));
    }
    
    public void HandlePortalAnimation()
    {
        Sprite previousSprite = currentSprite.sprite;

        RoomTransition.Direction direction = GetPlayerDirection();

        currentSprite.sprite = direction switch
        {
            RoomTransition.Direction.Up => upPortalAttack,
            RoomTransition.Direction.Down => downPortalAttack,
            RoomTransition.Direction.Left => leftPortalAttack,
            RoomTransition.Direction.Right => rightPortalAttack,
            _ => rightBowAttack
        };

        StartCoroutine(ResetSpriteAfterAttack(previousSprite));
    }

    private IEnumerator ResetSpriteAfterAttack(Sprite previousSprite)
    {
        yield return new WaitForSeconds(attackAnimationDuration);

        currentSprite.sprite = previousSprite;

        leftSwordCollider.enabled = false;
        rightSwordCollider.enabled = false;
        upSwordCollider.enabled = false;
        downSwordCollider.enabled = false;
    }
    
    
    private void EnableSwordCollider(RoomTransition.Direction dir)
    {
        leftSwordCollider.enabled = false;
        rightSwordCollider.enabled = false;
        upSwordCollider.enabled = false;
        downSwordCollider.enabled = false;

        switch (dir)
        {
            case RoomTransition.Direction.Up:
                upSwordCollider.enabled = true;
                break;
            case RoomTransition.Direction.Down:
                downSwordCollider.enabled = true;
                break;
            case RoomTransition.Direction.Left:
                leftSwordCollider.enabled = true;
                break;
            case RoomTransition.Direction.Right:
                rightSwordCollider.enabled = true;
                break;
        }
    }
    private Dictionary<Sprite, RoomTransition.Direction> spriteDirectionMap; 
    public RoomTransition.Direction GetPlayerDirection()
    {
        return spriteDirectionMap.TryGetValue(currentSprite.sprite, out var direction)
            ? direction
            : RoomTransition.Direction.Right; // Default direction
    } 

    public IEnumerator GameOverAnimation(float timeToAnimate)
    {
        // rotate player 3 times across timeToAnimate
        float rotateTime = Time.time + timeToAnimate;

        for (int i = 0; i < 12; i++)
        {
            while (Time.time < rotateTime)
            {
                yield return null;
            }

            if (currentSprite.sprite == downIdle)
            {
                currentSprite.sprite = rightIdle;
            }
            else if (currentSprite.sprite == rightIdle)
            {
                currentSprite.sprite = upIdle;
            }
            else if (currentSprite.sprite == upIdle)
            {
                currentSprite.sprite = leftIdle;
            }
            else
            {
                currentSprite.sprite = downIdle;
            }

            rotateTime = Time.time + timeToAnimate;
        }

        currentSprite.sprite = downIdle;
    }
}
