using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public float movement_speed = 2f;

    private Rigidbody rb;
    private int direction_index;
    private float timeSinceDirectionChange;
    private float directionChangeRate = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ChangeDirection(Random.Range(0, 4));
        timeSinceDirectionChange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;

        if (timeSinceDirectionChange >= directionChangeRate)
        {
            ChangeDirection(Random.Range(0, 4));
            timeSinceDirectionChange = 0;
        }
    }

    // up is 0, right is 1, down is 2, left is 3
    void ChangeDirection(int direction_index)
    {
        Vector3 currentVelocity = Vector2.zero;
        Vector2 currentPos = rb.position;

        Debug.Log(direction_index);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
