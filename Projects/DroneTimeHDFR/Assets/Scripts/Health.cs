using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth = 100f;
    public float currentHealth;
    public UIManager uiManager;

    void Start() {
        currentHealth = maxHealth;
    }

    public void TakeBulletDamage(float amount, GameObject droneObject) {
        currentHealth -= amount;
        if (gameObject.CompareTag("Player")) {
            uiManager.updateHealth(droneObject);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        DroneDestruction droneDestruction = GetComponent<DroneDestruction>();
        droneDestruction.ExplodeDrone();
        if (gameObject.CompareTag("Player")) {
            GameManager.singleton.respawnPlayer();
        }
        Destroy(gameObject); // Destroy the object when health reaches zero
    }

    public void setUI(GameObject uiObject) {
        uiManager = uiObject.GetComponent<UIManager>();
    }
}