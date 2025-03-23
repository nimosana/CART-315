using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public float timeRemaining = 60;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI enemiesText;
    public Slider healthSlider;
    public Slider ammoSlider;

    void Update() {
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
        }
        else {
            GameManager.singleton.wave++;
            GameManager.singleton.spawnWaves();
            timeRemaining = 60;
        }

        UpdateWaveDisplay(timeRemaining);
    }

    public void updateHealth(GameObject player) {
        Health health = player.gameObject.GetComponent<Health>();
        healthSlider.value = (health.currentHealth / GameManager.singleton.playerMaxHealth);
        // if (healthText != null) {
        //     healthText.text = "Health: " + $"{healthSlider.value * 100}";
        // }
    }

    public void updateAmmo(GameObject player) {
        DroneMovement droneStuff = player.GetComponent<DroneMovement>();
        ammoSlider.value = (droneStuff.playerAmmo / GameManager.singleton.playerMaxAmmo);
        if (ammoText != null) {
            ammoText.text = "Ammo: " + $"{droneStuff.playerAmmo}";
        }
    }

    void UpdateWaveDisplay(float timeToDisplay) {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = "Wave: " + $"{GameManager.singleton.wave}\n" + $"{minutes:00}:{seconds:00}";
        enemiesText.text = $"{GameManager.singleton.transform.childCount}" + " Hostiles";
    }
}