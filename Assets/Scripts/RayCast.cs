using System.Collections;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public float rayLength = 10.0f;
    public float horizontalDistance = 5f;
    public float verticalDistance = 2.5f;
    public float speed = 5f;

    private RaycastHit hit;

    private bool moving = false;

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            CastRay(Vector2.up);
            CastRay(Vector2.right);
            CastRay(Vector2.left);
            CastRay (Vector2.down);
        }
    }

    void CastRay(Vector2 direction)
    {
        if (Physics.Raycast(transform.position, direction, out hit, rayLength))
        {
            if (hit.collider.CompareTag("Player") && gameObject.name == "BladeTrap" && !moving)
            {
                Vector3 destination = hit.normal * -1;
                destination.x = destination.x * horizontalDistance;
                destination.y = destination.y * verticalDistance;
                StartCoroutine(AttackMove(transform.position + destination));
            }
        }
    }

    IEnumerator AttackMove(Vector3 destination)
    {
        moving = true;

        Vector3 startPosition = transform.position;

        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = destination;

        StartCoroutine(ReturnMove(startPosition));
    }

    IEnumerator ReturnMove(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime / 2);
            yield return null;
        }
        transform.position = destination;

        moving = false;
    }
}
