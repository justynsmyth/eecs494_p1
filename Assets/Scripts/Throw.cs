using UnityEngine;

public class Throw : MonoBehaviour
{
    public Boomerang boomerangWeapon;
    public GameObject boomerangPrefab;
    public float throwInterval = 2f;
    public float boomerangCooldown = 1f;
    private float nextThrowTime = 0f;

    private void Start()
    {
        boomerangWeapon.Setup(boomerangPrefab, boomerangCooldown, gameObject);
    }

    private void Update()
    {
        if (Time.time >= nextThrowTime && !boomerangWeapon.IsOnCooldown)
        {
            ThrowBoomerang();
            nextThrowTime = Time.time + throwInterval;
        }
    }

    private void ThrowBoomerang()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        RoomTransition.Direction direction = RoomTransition.Direction.Up; // does not matter
        boomerangWeapon.Attack(position, rotation, direction);
    }
}
