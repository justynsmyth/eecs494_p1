using UnityEngine;
using System.Collections;

public enum PushType
{
    Vertical,
    Horizontal
}
public class PushBlock : MonoBehaviour
{
    private Collider col;
    public float transformMagnitude = 1f;
    public bool bePushedInfinitely = false;
    public bool isMultiDirectional = false;

    public PushType pushType;
    public float moveDuration = 0.05f;
    
    public AudioClip pushAudio;

    public UnlockDoor door;

    public RoomClear room;

    private bool isMoving = false;
    private bool isPushed = false;

    private float holdTime = 0.0f;
    private Vector3 startPosition;
    

    void Start()
    {
        col = GetComponent<Collider>();
        startPosition = transform.position;
    }

    // Called by others to reset the block back to default
    public void ResetBlock()
    {
        StartCoroutine(ResetAfterDelay());
    }
    void OnCollisionStay(Collision collision)
    {
        if (isMoving || isPushed) return;
        if (room != null && !room.CheckIfCleared())
        {
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // get delta to current position (NEWS)
            Vector3 inputDirection = GetInputDirection();

            if (inputDirection != Vector3.zero)
            {
                holdTime += Time.deltaTime;

                if (holdTime >= moveDuration || bePushedInfinitely)
                {
                    Vector3 newPosition = CalculateDirection(inputDirection, transformMagnitude);

                    if (!IsMoveBlocked(newPosition, inputDirection))
                    {
                        if (pushAudio != null)
                        {
                            AudioSource.PlayClipAtPoint(pushAudio, Camera.main.transform.position);
                        }

                        StartCoroutine(MoveBlock(inputDirection));

                        if (door != null)
                        {
                            door.DoorUnlock();
                        }
                    }
                }
            }
            else
            {
                holdTime = 0.0f;
            }
        }
    }
    
    private IEnumerator MoveBlock(Vector3 direction)
    {
        isMoving = true;
        PlayerInput.IsActionInProgress = true;
        Vector3 origin = transform.position;
        Vector3 dest = CalculateDirection(direction, transformMagnitude);

        float time = 0.0f;
        while (time < moveDuration)
        {
            transform.position = Vector3.Lerp(origin, dest, time / moveDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = dest;
        if (!bePushedInfinitely) { isPushed = true;}
        isMoving = false;
        PlayerInput.IsActionInProgress = false;
    }
    
    private Vector3 CalculateDirection(Vector3 deltaDirection, float magnitude)
    {
        Vector3 newPos = transform.position;
        newPos += deltaDirection * magnitude;
        return newPos;
    }
    
    private Vector3 GetInputDirection()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        if (isMultiDirectional)
        {
            if (inputH != 0)
            {
                return Vector3.right * inputH;
            }
            if (inputV != 0)
            {
                return Vector3.up * inputV;
            }
            return new Vector3();
        }
        if (pushType == PushType.Vertical)
        {
            return Vector3.up * inputV;
        }
        return Vector3.right * inputH;
    }
    
    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(RoomTransition.transitionTime);
        transform.position = startPosition;
        isPushed = false;
        holdTime = 0.0f; 
    }
    
    // Checks to see if block can successfully move without getting blocked by the following scenarios
    // 1: Blocks with tags Wall or Doorway
    // 2: Blocks defined as Block layer
    // 3: If the player is in the way of the movement, we need to assure the player does not get crushed.
    //   Checks the next tile to see if there is space for the player to move to
    private bool IsMoveBlocked(Vector3 newPosition, Vector3 inputDirection)
    {
        if (CheckCollisions(newPosition)) 
        { 
            return true;
        }
        Collider[] potentialCollisions = Physics.OverlapBox(newPosition, col.bounds.extents, Quaternion.identity);
        foreach (var hitCollider in potentialCollisions)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vector3 nextPosition = CalculateDirection(inputDirection, transformMagnitude * 2);
                if (CheckCollisions(nextPosition))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CheckCollisions(Vector3 position)
    {
        Collider[] requiredCollisions = Physics.OverlapBox(position, col.bounds.extents, Quaternion.identity);

        foreach (var hitCollider in requiredCollisions)
        {
            if (hitCollider.CompareTag("Wall") || hitCollider.CompareTag("Doorway"))
            {
                return true;
            }
            if ((hitCollider.gameObject.layer == LayerMask.NameToLayer("Block") ||
                hitCollider.gameObject.layer == LayerMask.NameToLayer("Water"))
                && hitCollider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }
}