using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DroneDestruction : MonoBehaviour {
    public VisualEffect explosionEffect;
    public Transform[] droneParts;
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public Rigidbody rb;


    public void ExplodeDrone() {
        VisualEffect effectInstance = Instantiate(explosionEffect, rb.position, Quaternion.identity);
        Destroy(effectInstance.gameObject, 5f);
        effectInstance.Play();

        // Detach and apply forces to all child rigidbodies
        foreach (Transform part in droneParts) {
            part.SetParent(null);

            Rigidbody partRb = part.GetComponent<Rigidbody>();
            if (!partRb) partRb = part.gameObject.AddComponent<Rigidbody>();

            partRb.linearVelocity = partRb.linearVelocity;
            partRb.angularVelocity = partRb.angularVelocity;
            partRb.AddExplosionForce(explosionForce, partRb.position, explosionRadius);

            Destroy(part.gameObject, 4f);
        }
    }
}