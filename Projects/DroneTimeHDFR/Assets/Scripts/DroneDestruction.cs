using UnityEngine;
using UnityEngine.VFX;

public class DroneDestruction : MonoBehaviour {
    public VisualEffect explosionEffect;
    public Transform[] droneParts;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public Rigidbody droneRb;

    public void ExplodeDrone() {
        VisualEffect effectInstance = Instantiate(explosionEffect, droneRb.position, Quaternion.identity);
        Destroy(effectInstance.gameObject, 5f);
        effectInstance.Play();
        // Detach and apply forces to all child rigidbodies
        foreach (Transform part in droneParts) {
            part.SetParent(null);

            Rigidbody rb = part.GetComponent<Rigidbody>();
            Collider col = part.GetComponent<Collider>();
            if (rb == null) rb = part.gameObject.AddComponent<Rigidbody>();
            // if (col == null) col = part.gameObject.AddComponent<Collider>();

            rb.linearVelocity = droneRb.linearVelocity;
            rb.angularVelocity = droneRb.angularVelocity;
            rb.AddExplosionForce(explosionForce, droneRb.position, explosionRadius);

            Destroy(part.gameObject, 4f);
        }
    }
}