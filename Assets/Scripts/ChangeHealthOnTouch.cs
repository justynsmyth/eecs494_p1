using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class ChangeHealthOnTouch : MonoBehaviour
{
    public float health_change_amount;
    public float knockback_power;
    public bool destroy_self_on_touch = false;

    public void OnTriggerEnter(Collider other)
    {
        /* Adjust hp */
        HasHealth other_health = other.GetComponent<HasHealth>();
        if (other_health == null)
            return;

        other_health.AlterHealth(health_change_amount);
        Debug.Log(other.gameObject.name.ToString() + " took damage!");

        /* Perform Knockback */
        Rigidbody other_rb = other.GetComponent<Rigidbody>();
        if (other_rb != null)
        {
            Vector3 knockback_direction = (other.transform.position - transform.position).normalized;
            other_rb.linearVelocity = knockback_direction * knockback_power;
        }

        /* Destroy self */
        if (destroy_self_on_touch)
            Destroy(gameObject);
    }
}