using System;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject objectPrefab;
    public float throwMinInterval = 2f;
    public float throwMaxInterval = 4f;

    private float throwInterval;
    private GameObject currentBoomerang;
    private float nextThrowTime = 0f;
    private bool isThrowing = false;
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement component is not attached to the GameObject.");
        }
    }

    private void Update()
    {
        if (Time.time >= nextThrowTime)
        {
            StartThrowing();
        }
    }

    private void FixedUpdate()
    {
        if (isThrowing && (currentBoomerang == null || !currentBoomerang.activeInHierarchy))
        {
            FinishThrowing();
        }
    }

    private void StartThrowing()
    {
        isThrowing = true;
        enemyMovement.stopMovement = true;
        ThrowBoomerang();
        throwInterval = UnityEngine.Random.Range(throwMinInterval, throwMaxInterval);
        nextThrowTime = Time.time + throwInterval;
    }

    private void FinishThrowing()
    {
        isThrowing = false;
        enemyMovement.stopMovement = false;
    }
    
    private void ThrowBoomerang()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        currentBoomerang = Instantiate(objectPrefab, position, rotation);
        
        BoomerangProjectile projectile = currentBoomerang.GetComponent<BoomerangProjectile>();
        if (projectile != null)
        {
            projectile.Setup(gameObject);
        }
    }
}