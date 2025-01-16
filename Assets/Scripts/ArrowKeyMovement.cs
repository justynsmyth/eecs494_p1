using System;
using UnityEngine;

public class ArrowKeyMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float movement_speed = 4f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sets the Player Rigidbody to the rb variable
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!RoomTransition.IsTransitionInProgress())
        {
            Vector2 currentInput = GetInput();
            Vector3 currentPos = rb.position;
            rb.linearVelocity = currentInput * movement_speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
 

    Vector2 GetInput()
    {
        // Take horizontal movement inputs (A, D, <-, ->)
        float horizontal_input = Input.GetAxisRaw("Horizontal");

        // Take vertical movement inputs (W, S, up, down)
        float vertical_input = Input.GetAxisRaw("Vertical");

        // Remove diagonal movement
        // If we're moving horizontally, don't allow vertical movement
        if (Mathf.Abs(horizontal_input) > 0.0f)
        {
            vertical_input = 0.0f;
        }

        return new Vector2(horizontal_input, vertical_input);
    }
}
