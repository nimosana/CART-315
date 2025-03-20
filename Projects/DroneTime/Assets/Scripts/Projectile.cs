using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage = 10f;

    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject); // Destroy bullet on impact
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) {
            health.TakeDamage(damage, collision.gameObject);

            DroneDestruction droneDestruction = collision.gameObject.GetComponent<DroneDestruction>();
            if (droneDestruction != null && health.currentHealth <= 0) {
                droneDestruction.ExplodeDrone();
                Destroy(collision.gameObject);
            }
        }
    }
}