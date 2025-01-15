using UnityEngine;

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

    public float moveRate = 0.15f;

    private SpriteRenderer currentSprite;
    private float timeSinceAnimating;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSprite = GetComponent<SpriteRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Changing direction looks weird because the sprite won't immediately update if you switch
        timeSinceAnimating += Time.deltaTime;

        // Take horizontal movement inputs (A, D, <-, ->)
        float horizontal_input = Input.GetAxisRaw("Horizontal");

        // Take vertical movement inputs (W, S, up, down)
        float vertical_input = Input.GetAxisRaw("Vertical");

        // Update the sprite here
        // Moving right
        if (horizontal_input > 0.0f)
        {
            if (currentSprite.sprite == rightMoving && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = rightIdle;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite == rightIdle && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = rightMoving;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite != rightIdle && currentSprite != rightMoving)
            {
                currentSprite.sprite = rightMoving;
            }
        }
        // Moving left
        else if (horizontal_input < 0.0f)
        {
            if (currentSprite.sprite == leftMoving && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = leftIdle;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite == leftIdle && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = leftMoving;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite != leftIdle && currentSprite != leftMoving)
            {
                currentSprite.sprite = leftMoving;
            }
        }
        // Moving up
        else if (vertical_input > 0.0f)
        {
            if (currentSprite.sprite == upMoving && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = upIdle;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite == upIdle && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = upMoving;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite != upIdle && currentSprite != upMoving)
            {
                currentSprite.sprite = upMoving;
            }
        }
        // Moving down
        else if (vertical_input < 0.0f)
        {
            if (currentSprite.sprite == downMoving && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = downIdle;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite == downIdle && timeSinceAnimating >= moveRate)
            {
                currentSprite.sprite = downMoving;
                timeSinceAnimating = 0.0f;
            }
            else if (currentSprite.sprite != downIdle && currentSprite != downMoving)
            {
                currentSprite.sprite = downMoving;
            }
        }
    }
}
