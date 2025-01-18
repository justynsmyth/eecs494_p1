using System;
using UnityEngine;

public class PlayerInput  : MonoBehaviour
{
    private Rigidbody rb;

    public GameManager manager;
    public float movement_speed = 4f;
    public bool control = true;

    void Start()
    {
        // Sets the Player Rigidbody to the rb variable
        rb = GetComponent<Rigidbody>();
    }

    // Use .invoke() to publish/trigger a message to subscribed classes
    public static event Action OnSpacePressed;
    public static event Action OnXPressed;
    public static event Action OnZPressed;
    
    // Subscribing to an Event via '+='
    // PlayerInput.OnSpacePressed += FunctionNameHere()
    // PlayerInput.OnXPressed += () => Debug.Log("X Was Pressed"); // Lambda
    // Unsubscribing on Destruction via '-='
    
    void Update()
    {
        if (control)
        {
            HandleInputMovement();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpacePressed?.Invoke();
                Debug.Log("Space Key Pressed");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                OnXPressed?.Invoke();
                Debug.Log("X Key Pressed");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnZPressed?.Invoke();
                Debug.Log("Z Key Pressed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                manager.CheatModeToggle();
                Debug.Log("1 Key Pressed");
            }
        }
    }

    private void HandleInputMovement()
    {
        if (!RoomTransition.IsTransitionInProgress())
        {
            Vector2 currentInput = GetInput();
            Vector3 currentVelocity = Vector2.zero;
            Vector2 currentPos = rb.position;

            if (currentInput.x != 0)
            {
                currentVelocity.x += currentInput.x * movement_speed;
                currentVelocity.y += AlignToGrid(currentPos.y, 0.5f) / Time.fixedDeltaTime;
            } else if (currentInput.y != 0)
            {
                currentVelocity.x += AlignToGrid(currentPos.x, 0.5f) / Time.fixedDeltaTime;
                currentVelocity.y += currentInput.y * movement_speed;
            }
            rb.linearVelocity = currentVelocity; 
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    /// <summary>
    /// Takes a position and adjusts it to align with a factor value.
    /// </summary>
    /// <param name="val">X or Y position of a player</param>
    /// <param name="factor">Expected Grid Size to 'Snap' to</param>
    private float AlignToGrid(float val, float factor)
    {
        float r = val % factor;
        float h = factor / 2;
        if (r < h)
        {
            return -r;
        }

        return factor - r;
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
