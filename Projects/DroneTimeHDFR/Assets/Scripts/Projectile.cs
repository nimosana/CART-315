using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage = 10f;

    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject); // Destroy bullet on impact
        Health health = collision.gameObject.GetComponent<Health>();

        if (health) {
            health.TakeBulletDamage(damage, collision.gameObject);
        }
    }
}