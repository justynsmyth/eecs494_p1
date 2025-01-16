using System;
using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    private Camera cam;
    public float transitionTime = 2.5f;

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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(direction);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction == Direction.Up)
            {
                StartCoroutine(TransitionCamera(Direction.Up));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction == Direction.Down)
            {
                StartCoroutine(TransitionCamera(Direction.Down));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction == Direction.Left)
            {
                StartCoroutine(TransitionCamera(Direction.Left));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction == Direction.Right)
            {
                StartCoroutine(TransitionCamera(Direction.Right));
            };
        }    
    }

    private IEnumerator TransitionCamera(Direction moveDirection)
    {
        
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = startPos + GetDirectionVector(moveDirection);
        Debug.Log($"Start position: {startPos}, End position: {endPos}");

        yield return StartCoroutine(
            MoveObjectOverTime(
                cam.transform, startPos, endPos, transitionTime)
        );
    }

    public static Vector3 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3(0, 20, 0);
            case Direction.Down:
                return new Vector3(0, -20, 0);
            case Direction.Left:
                return new Vector3(-20, 0, 0);
            case Direction.Right:
                return new Vector3(20, 0, 0);
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
