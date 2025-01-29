using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float attackSpeed = 5.0f;
    public float projectileSpeed = 3.0f;
    public GameObject projectile;
    public GameObject player;
    public float projectileSpread = 5.0f;

    private float timeSinceAttack = 0.5f;

    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;

        if (timeSinceAttack >= attackSpeed)
        {
            timeSinceAttack = 0;
            Attack();
        }
    }

    void Attack()
    {
        GameObject projectile_normal = Instantiate(projectile, transform.position, transform.rotation);
        GameObject projectile_up = Instantiate(projectile, transform.position, transform.rotation);
        GameObject projectile_down = Instantiate(projectile, transform.position, transform.rotation);

        Vector3 direction = AttackDirection(player.transform.position);
        projectile_normal.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;

        direction = AttackDirection(player.transform.position + (Vector3.up * projectileSpread));
        projectile_up.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;

        direction = AttackDirection(player.transform.position + (Vector3.down * projectileSpread));
        projectile_down.GetComponent<Rigidbody>().linearVelocity = direction * projectileSpeed;
    }

    Vector3 AttackDirection(Vector3 targetPosition)
    {
        return (targetPosition - gameObject.transform.position).normalized;
    }
}
