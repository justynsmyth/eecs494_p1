using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    public float movement_speed = 2f;
    public float directionChangeRate = 2f;
    public float spriteChangeRate = 0.15f;
    public float timeCollisionDirectionChange = 1f;
    public Sprite movingSprite;


    private Rigidbody rb;
    private float timeSinceDirectionChange;
    private float timeSinceAnimation;
    private SpriteRenderer enemySprite;
    private Sprite defaultSprite;

    private HasHealth hasHealth;
    private Vector3 currentDirection;

    private bool gelPause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasHealth = GetComponent<HasHealth>();
        enemySprite = GetComponent<SpriteRenderer>();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;

        ChangeDirection();
        currentDirection = rb.linearVelocity;

        directionChangeRate = Random.Range(0f, 3f);
        timeSinceDirectionChange = 0;

        gelPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;
        timeSinceAnimation += Time.deltaTime;

        if (timeSinceAnimation >= spriteChangeRate)
        {
            AnimateSprite();

            timeSinceAnimation = 0;
        }

        if (hasHealth.IsInvulnerable)
        {
            return;
        }
        if (timeSinceDirectionChange >= directionChangeRate)
        {
            if (gameObject.name == "Gel")
            {
                directionChangeRate = 1f;
                if (gelPause)
                {
                    rb.linearVelocity = Vector2.zero;
                    gelPause = false;
                }
                else
                {
                    ChangeDirection();
                    gelPause = true;
                }
            }
            else
            {
                do
                {
                    ChangeDirection();
                } while (rb.linearVelocity == currentDirection);
                currentDirection = rb.linearVelocity;

                directionChangeRate = Random.Range(0f, 3f);
            }

            timeSinceDirectionChange = 0;
        }
    }

    void FixedUpdate()
    {
        ApplyGridSnap(); // need to call at FixedTick so GridSnap can adjust Enemy's location each FixedFrame.
    }

    // If the enemy has a sprite when animated, it will alternate between that sprite and its default sprite. Otherwise, it will flip sprite direction
    void AnimateSprite()
    {
        if (movingSprite != null)
        {
            if (enemySprite.sprite == defaultSprite)
            {
                enemySprite.sprite = movingSprite;
            }
            else
            {
                enemySprite.sprite = defaultSprite;
            }
        }
        else
        {
            if (enemySprite.flipX)
            {
                enemySprite.flipX = false;
            }
            else
            {
                enemySprite.flipX = true;
            }
        }
    }

    // up is 0, right is 1, down is 2, left is 3
    void ChangeDirection()
    {
        Vector3 currentVelocity = Vector2.zero;

        float xValue = Random.Range(-1f, 1f);
        float yValue = Random.Range(-1f, 1f);

        if (gameObject.name == "Aquamentus")
        {
            yValue = 0f;
        }
        else if (gameObject.name != "Keese")
        {
            int horizontal = Random.Range(0, 2);

            if (horizontal == 1)
            {
                yValue = 0f;
            }
            else
            {
                xValue = 0f;
            }
        }

        currentVelocity.x = xValue;
        currentVelocity.y = yValue;

        // TODO: Keese 2D vector movement not being normalized correctly
        rb.linearVelocity = currentVelocity.normalized * movement_speed;
    }

    private void ApplyGridSnap()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 currentPos = rb.position;
        float alignFactor = 0.5f;

        if (gameObject.name == "Gel")
        {
            alignFactor = 1f;
        }

        if (currentVelocity.x != 0)
        {
            currentPos.y = Mathf.Lerp(currentPos.y, AlignToGridPosition(currentPos.y, alignFactor), Time.deltaTime * movement_speed);
        }
        else if (currentVelocity.y != 0)
        {
            currentPos.x = Mathf.Lerp(currentPos.x, AlignToGridPosition(currentPos.x, alignFactor), Time.deltaTime * movement_speed);
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

    private float lastCollisionDirectionChangeTime;

    private void OnCollisionEnter(Collision collision)
    {
        // If an Enemy collides with anything, try to change directions (to maintain movement)
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            if (Time.time >= timeCollisionDirectionChange + lastCollisionDirectionChangeTime)
            {
                do
                {
                    ChangeDirection();
                } while (rb.linearVelocity == currentDirection);
                currentDirection = rb.linearVelocity;

                ChangeDirection();
                lastCollisionDirectionChangeTime = Time.time;
            }
        }
    }
}