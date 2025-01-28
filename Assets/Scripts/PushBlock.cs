using UnityEngine;
using System.Collections;

public enum PushType
{
    Vertical,
    Horizontal
}
public class PushBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector3 transformDirection;
    public float playerPushDirection = 1.0f;

    public PushType pushType;
    public float moveDuration = 0.05f;
    

    private bool isMoving = false;
    private bool isPushed = false;

    private float holdTime = 0.0f;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // Called by others to reset the block back to default
    public void ResetBlock()
    {
        StartCoroutine(ResetAfterDelay());
    }
    void OnCollisionStay(Collision collision)
    {
        if (isPushed) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Pushing this way: " + Input.GetAxisRaw(pushType.ToString()));
            if (Input.GetAxisRaw(pushType.ToString()) == playerPushDirection && !isMoving)
            {
                holdTime += Time.deltaTime;

                if (holdTime >= moveDuration)
                {
                    StartCoroutine(MoveBlock());
                }
            }
            else
            {
                holdTime = 0.0f;
            }
        }
    }

    private IEnumerator MoveBlock()
    {
        isMoving = true;
        PlayerInput.IsActionInProgress = true;
        Vector3 origin = transform.position;
        Vector3 dest = origin + transformDirection;

        float time = 0.0f;
        while (time < moveDuration)
        {
            transform.position = Vector3.Lerp(origin, dest, time / moveDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = dest;
        isPushed = true;
        isMoving = false;
        PlayerInput.IsActionInProgress = false;
    }
    
    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(RoomTransition.transitionTime);
        transform.position = startPosition;
        isPushed = false;
        holdTime = 0.0f; 
    }
}
