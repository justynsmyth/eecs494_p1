using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    private Camera cam;
    public static float transitionTime = 2f;

    public float cameraDistanceY = 11f;
    public float cameraDistanceX = 16f;
    public float playerAutoWalkDistance = 2f;
    
    // used to inform ArrowKeyMovement.cs to stop moving during transition
    public static bool isTransitionInProgress = false;

    public GameObject PushBlock;
    
    public static bool IsTransitionInProgress()
    {
        return isTransitionInProgress;
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };
    
    public Direction direction;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main; 
        }
    }
    
    /// <summary>
    /// Uses the playerAnimator's face direction to determine which room the player intends to enter.
    /// Since the player overlaps the returning trigger when entering the next room, IsOppositeDirection will
    /// prevent retriggering a camera transition.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Rigidbody rb = other.GetComponent<Rigidbody>();
            InputToAnimator playerAnimator = other.GetComponent<InputToAnimator>();
            Direction playerDirection = playerAnimator.GetPlayerDirection();

            if (IsOppositeDirection(playerDirection))
            {
                return;
            }
            
            rb.linearVelocity = Vector3.zero;

            StartCoroutine(TransitionCameraAndMovePlayer(direction, other.gameObject));

            if (PushBlock != null)
            {
                var pb = PushBlock.GetComponent<PushBlock>();
                if (pb != null)
                {
                    pb.ResetBlock();
                } 
            }
        }    
    }

    /// <summary>
    /// Determine if a trigger's set camera transition direction is opposite to player's sprite direction
    /// </summary>
    /// <param name="playerDirection"></param>
    /// <returns></returns>
    private bool IsOppositeDirection(Direction playerDirection)
    {
        if (direction == Direction.Right) { return playerDirection == Direction.Left; }
        if (direction == Direction.Left) { return playerDirection == Direction.Right; }
        if (direction == Direction.Up) { return playerDirection == Direction.Down; }
        if (direction == Direction.Down) { return playerDirection == Direction.Up; }
        return false;
    }

    private IEnumerator TransitionCameraAndMovePlayer(Direction moveDirection, GameObject player)
    {
        isTransitionInProgress = true;
        PortalManager.instance.DestroyPortals();
        player.GetComponent<SpriteRenderer>().enabled = false;
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = startPos + GetDirectionVector(moveDirection);

        yield return StartCoroutine(
            MoveObjectOverTime(
                cam.transform, startPos, endPos, transitionTime)
        );

        player.GetComponent<SpriteRenderer>().enabled = true;

        yield return StartCoroutine(MovePlayerForwardForDuration(player, moveDirection, 0.5f));

        isTransitionInProgress = false;
    }
    
    /// <summary>
    /// Translates the player's position (playerAutoWalkDistance) in the moveDirection via linear interpolation.
    /// Chose to use player transform over linearVelocity because
    /// the player would have a small tendency to get stuck during transition for some reason.
    /// </summary>
    private IEnumerator MovePlayerForwardForDuration(GameObject player, Direction moveDirection, float duration)
    {
        Transform playerTransform = player.transform;
    
        Vector3 movement = moveDirection switch
        {
            Direction.Up => Vector3.up * playerAutoWalkDistance,
            Direction.Down => Vector3.down * playerAutoWalkDistance,
            Direction.Left => Vector3.left * playerAutoWalkDistance,
            Direction.Right => Vector3.right * playerAutoWalkDistance,
            _ => Vector3.zero
        };

        Vector3 targetPosition = playerTransform.position + movement;

        float startTime = Time.time;
        Vector3 startPosition = playerTransform.position;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        playerTransform.position = targetPosition;
    }
 


    private Vector3 GetDirectionVector(Direction moveDirection)
    {
        switch (moveDirection)
        {
            case Direction.Up:
                return new Vector3(0, cameraDistanceY, 0);
            case Direction.Down:
                return new Vector3(0, -cameraDistanceY, 0);
            case Direction.Left:
                return new Vector3(-cameraDistanceX, 0, 0);
            case Direction.Right:
                return new Vector3(cameraDistanceX, 0, 0);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    IEnumerator MoveObjectOverTime(Transform t, Vector3 startPos, Vector3 endPos, float time)
    {
        float startTime = Time.time;
        float progress = (Time.time - startTime) / time;
        while (progress < 1.0f)
        {
            progress = (Time.time - startTime) / time;
            Vector3 pos = Vector3.Lerp(startPos, endPos, progress);
            
            t.position = pos;
            yield return null;
        }
        
        t.position = endPos;
    }
}
