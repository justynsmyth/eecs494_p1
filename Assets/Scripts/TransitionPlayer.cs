using UnityEngine;

public class TransitionPlayer : MonoBehaviour
{
    public float xValue;
    public float yValue;

    public float xCamera;
    public float yCamera;
    private float zCamera = -10f;

    public Sprite playerSprite;

    private Camera cam;
    private Vector3 destination;
    private Vector3 cameraPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destination.x = xValue;
        destination.y = yValue;

        cameraPosition.x = xCamera;
        cameraPosition.y = yCamera;
        cameraPosition.z = zCamera;

        cam = Camera.main;
    }

    // TODO: make this into a coroutine similar to roomtransition
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = destination;
            cam.transform.position = cameraPosition;

            other.gameObject.GetComponent<PlayerInput>().control = false;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = playerSprite;

            other.gameObject.GetComponent<PlayerInput>().control = true;
        }
    }
}
