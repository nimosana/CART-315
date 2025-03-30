using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth = 100f;
    public float currentHealth;
    public UIManager uiManager;
    public bool exploded;

    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeBulletDamage(float amount, GameObject droneObject) {
        if (!gameObject || exploded) return;
        currentHealth -= amount;
        if (gameObject.CompareTag("Player")) {
            uiManager.UpdatePlayerHealth(droneObject);
        }

        if (currentHealth <= 0 && !exploded) {
            if (!exploded && Random.value < 0.5f) {
                exploded = true;
                TriggerSuicide();
                return;
            }

            Die();
        }
    }

    public void TakeDamage(float amount) {
        if (!gameObject || exploded) return;
        currentHealth -= amount;
        if (gameObject.CompareTag("Player")) {
            uiManager.UpdatePlayerHealth(gameObject);
        }

        if (currentHealth <= 0 && !exploded) {
            if (!exploded && Random.value < 0.5f) {
                exploded = true;
                TriggerSuicide();
                return;
            }

            Die();
        }
    }

    // trigger explosion.
    private void TriggerSuicide() {
        if (gameObject.CompareTag("EnemyDrone")) {
            EnemyDrone enemyScript = GetComponent<EnemyDrone>();
            if (enemyScript) {
                exploded = true;
                enemyScript.SuicideDrone();
                return;
            }
        }
        else if (gameObject.CompareTag("Player")) {
            DroneMovement playerDrone = GetComponent<DroneMovement>();
            if (playerDrone) {
                exploded = true;
                playerDrone.SuicideDrone();
                return;
            }
        }

        Die();
    }

    public void BaseDamage(float damage) {
        currentHealth -= damage;
    }

    public void Die() {
        exploded = true;
        DroneDestruction droneDestruction = GetComponent<DroneDestruction>();
        if (droneDestruction) {
            droneDestruction.ExplodeDrone();
        }

        if (gameObject.CompareTag("Player") && GameManager.singleton.playerAlive) {
            GameManager.singleton.RespawnPlayer();
            GameManager.singleton.playerAlive = false;
        }

        Destroy(gameObject);
    }

    public void SetUI(GameObject uiObject) {
        uiManager = uiObject.GetComponent<UIManager>();
    }
}