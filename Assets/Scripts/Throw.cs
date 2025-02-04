using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public float throwInterval = 2f;
    public float boomerangCooldown = 1f;
    private float nextThrowTime = 0f;

    private void Update()
    {
        if (Time.time >= nextThrowTime)
        {
            ThrowBoomerang();
            nextThrowTime = Time.time + throwInterval;
        }
    }

    private void ThrowBoomerang()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        GameObject boomerangInstance = Instantiate(boomerangPrefab, position, rotation);

        BoomerangProjectile projectile = boomerangInstance.GetComponent<BoomerangProjectile>();
        if (projectile != null)
        {
            projectile.Setup(gameObject);
        }
    }
}
