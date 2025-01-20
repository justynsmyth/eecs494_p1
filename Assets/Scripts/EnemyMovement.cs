using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    public float movement_speed = 2f;
    public float directionChangeRate = 2f;
    public float spriteChangeRate = 0.15f;

    private Rigidbody rb;
    private float timeSinceDirectionChange;
    private float timeSinceAnimation;
    private SpriteRenderer enemySprite;
    
    private ChangeHealthOnTouch changeHealthOnTouch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        changeHealthOnTouch = GetComponent<ChangeHealthOnTouch>();
        enemySprite = GetComponent<SpriteRenderer>();

        ChangeDirection(Random.Range(0, 4));
        directionChangeRate = Random.Range(0f, 3f);
        timeSinceDirectionChange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;
        timeSinceAnimation += Time.deltaTime;

        if (timeSinceAnimation >= spriteChangeRate) 
        { 
            if (enemySprite.flipX)
            {
                enemySprite.flipX = false;
            }
            else
            {
                enemySprite.flipX = true;
            }

            timeSinceAnimation = 0;
        }

        if (changeHealthOnTouch.IsInvulnerable)
        {
            return;
        }
        if (timeSinceDirectionChange >= directionChangeRate)
        {
            ChangeDirection(Random.Range(0, 4));
            directionChangeRate = Random.Range(0f, 3f);
            timeSinceDirectionChange = 0;
        }
    }

    void FixedUpdate()
    {
        ApplyGridSnap(); // need to call at FixedTick so GridSnap can adjust Enemy's location each FixedFrame.
    }

    // up is 0, right is 1, down is 2, left is 3
    void ChangeDirection(int direction_index)
    {
        Vector3 currentVelocity = Vector2.zero;

        if (direction_index == 0) 
        {
            currentVelocity.y = 1;
        }
        else if (direction_index == 1)
        {
            currentVelocity.x = 1;
        }
        else if (direction_index == 2)
        {
            currentVelocity.y = -1;
        }
        else
        {
            currentVelocity.x = -1;
        }
        rb.linearVelocity = currentVelocity * movement_speed;
    }
    
    private void ApplyGridSnap()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 currentPos = rb.position;

        if (currentVelocity.x != 0)
        {
            currentPos.y = Mathf.Lerp(currentPos.y, AlignToGridPosition(currentPos.y, 0.5f), Time.deltaTime * movement_speed);
        }
        else if (currentVelocity.y != 0)
        {
            currentPos.x = Mathf.Lerp(currentPos.x, AlignToGridPosition(currentPos.x, 0.5f), Time.deltaTime * movement_speed);
        }

        rb.MovePosition(currentPos);
    }

    /// <summary>
    /// Calculates the target grid position for alignment.
    /// </summary>
    /// <param name="val">The current position value (X or Y).</param>
    /// <param name="factor">The grid size to snap to.</param>
    /// <returns>The nearest grid position.</returns>
    private float AlignToGridPosition(float val, float factor)
    {
        return Mathf.Round(val / factor) * factor;
    }
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }
        // If an Enemy collides with anything, try to change directions (to maintain movement)
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            ChangeDirection(Random.Range(0,4));
        }
    }
}
