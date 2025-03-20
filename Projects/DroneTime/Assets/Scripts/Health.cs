using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth = 100f;
    public float currentHealth;
    public UIManager uiManager;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject droneObject) {
        currentHealth -= amount;
        if (gameObject.CompareTag("Player")) {
            uiManager.updateHealth(droneObject);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject); // Destroy the object when health reaches zero
    }
}