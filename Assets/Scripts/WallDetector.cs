using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public Vector3 displacement;
    public Vector3 initDirection;
    public Vector3 turnDirection;
    public float speed = 2f;
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        { 
           Vector3 spawnPos = gameObject.transform.position + displacement;
           GameManager.instance.SpawnEnemy(spawnPos, initDirection, turnDirection, speed);
        }
    } 
}
