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
    public float transformMagnitude = 1f;

    public PushType pushType;
    public float moveDuration = 0.05f;
    
    public AudioClip pushAudio;

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
            if (Input.GetAxisRaw(pushType.ToString()) != 0 && !isMoving)
            {
                holdTime += Time.deltaTime;

                if (holdTime >= moveDuration && transform.position == startPosition)
                {
                    AudioSource.PlayClipAtPoint(pushAudio, Camera.main.transform.position);
                    StartCoroutine(MoveBlock(Input.GetAxisRaw(pushType.ToString())));
                }
            }
            else
            {
                holdTime = 0.0f;
            }
        }
    }

    private IEnumerator MoveBlock(float direction)
    {
        isMoving = true;
        PlayerInput.IsActionInProgress = true;
        Vector3 origin = transform.position;
        Vector3 dest = origin;

        if (pushType.ToString() == "Horizontal")
        {
            dest.x = dest.x + (direction * transformMagnitude);
        }
        else
        {
            dest.y = dest.y + (direction * transformMagnitude);
        }

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
