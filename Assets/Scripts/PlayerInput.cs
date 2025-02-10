using System;
using UnityEngine;

public class PlayerInput  : MonoBehaviour
{
    private Rigidbody rb;

    public GameManager manager;
    public float movement_speed = 4f;
    public bool control = true;

    public static bool IsActionInProgress = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        manager = GameManager.instance;
    }

    private void OnEnable()
    {
        manager = GameManager.instance;
    }

    public static event Action OnSpacePressed;
    public static event Action OnXPressed;
    public static event Action OnZPressed;
    void Update()
    {
        if (control)
        {
            HandleMovement();
            if (!RoomTransition.IsTransitionInProgress() && !IsActionInProgress)
            {
                HandleInputActions();
            }
        }
    }

    private void HandleMovement()
    {
        // Stop Movement
        if (RoomTransition.IsTransitionInProgress() || IsActionInProgress)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }
        // Calculate New Velocity
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


    private void HandleInputActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            OnXPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnZPressed?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            manager.CheatModeToggle();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            manager.CheatModeToggle();
        }
    }
}
