using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WallMasterMover : MonoBehaviour
{
    private Vector3 moveDirection;
    private Vector3 turnDir;
    private Vector3 initDir;
    private Vector3 initLocation;
    private float speed;

    private int moveSteps;
    private float stepSize = 1f;
    private float distanceMoved = 0f;
    private int movementPhase;

    private bool isMoving = true;
    private GameObject player;

    public Sprite GrabSprite;

    public Vector3 teleportCameraPos = new Vector3(39.5f, 7f, -10f);
    public Vector3 teleportPlayerPos = new Vector3(40f, 2f, 0f);


    public void Initialize(Vector3 initDirection, Vector3 turnDirection, float moveSpeed)
    {
        initDir = initDirection.normalized;
        turnDir = turnDirection.normalized;
        speed = moveSpeed;
        initLocation = transform.position;

        moveDirection = initDir; // Start moving in the initial direction
        moveSteps = 1;
        movementPhase = 0;
    }

    void Update()
    {
        if (isMoving)
        {
            if (player != null)
            {
                player.transform.position = transform.position;
            }
            Move();
        }
    }

    private void Move()
    {
        float stepDistance = speed * Time.deltaTime;

        transform.position += moveDirection * stepDistance;
        distanceMoved += stepDistance;

        if (movementPhase == 0 && distanceMoved >= moveSteps * stepSize)
        {
            distanceMoved = 0f; // Reset distance tracker
            moveDirection = turnDir; // Change direction to turnDirection
            moveSteps = Random.Range(3, 5); // Set moveSteps for turnDirection
            movementPhase = 1;
        }
        else if (movementPhase == 1 && distanceMoved >= moveSteps * stepSize)
        {
            distanceMoved = 0f; // Reset distance tracker
            moveDirection = -initDir; // Change direction to the opposite of the initial direction
            moveSteps = Random.Range(3, 6); // Set moveSteps for the opposite direction
            movementPhase = 2;
        }
        else if (movementPhase == 2 && distanceMoved >= moveSteps * stepSize)
        {
            isMoving = false;
            TeleportPlayer();
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = GrabSprite;
            player = col.gameObject;
            player.GetComponent<BoxCollider>().enabled = false;
            col.GetComponent<PlayerInput>().control = false;
        }
    }

    private void TeleportPlayer()
    {
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        
            player.GetComponent<BoxCollider>().enabled = false;
            player.transform.position = teleportPlayerPos;
            player.GetComponent<BoxCollider>().enabled = true;// Re-enable collider
        
            Camera mainCamera = Camera.main;
            mainCamera.transform.position = teleportCameraPos;

            player.GetComponent<PlayerInput>().control = true;

            // Reactivate Rigidbody if applicable
            if (rb != null)
            {
                rb.isKinematic = false; // Make it non-kinematic again
            }
        }
        if (gameObject != null) Destroy(gameObject);
        GameManager.instance.usedSpawnLocations.Remove(initLocation);
    }

    private void OnDestroy()
    {
        GameManager.instance.enemies -= 1;
    }
}
