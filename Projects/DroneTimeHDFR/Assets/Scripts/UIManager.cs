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
    public Slider baseSlider;

    public Slider ammoSlider;
    public float baseHealth = 100f;
    public Health health;

    void Update() {
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
        }
        else if (timeRemaining <= 0 && GameManager.singleton.playerAlive) {
            GameManager.singleton.wave++;
            GameManager.singleton.SpawnWaves();
            timeRemaining = 60;
        }

        UpdateWaveDisplay(timeRemaining);
    }

    public void UpdatePlayerHealth(GameObject player) {
        health = player.gameObject.GetComponent<Health>();
        healthSlider.value = (health.currentHealth / GameManager.singleton.playerMaxHealth);
    }

    public void UpdateBaseHealth() {
        health = GameManager.singleton.baseInstance.GetComponent<Health>();
        baseSlider.value = health.currentHealth / health.maxHealth;
    }

    public void UpdateAmmo(float playerAmmo) {
        ammoSlider.value = (playerAmmo / GameManager.singleton.playerMaxAmmo);
        if (ammoText) {
            ammoText.text = "Ammo: " + $"{playerAmmo}";
        }
    }

    public void NewDroneUI(GameObject player) {
        UpdateAmmo(GameManager.singleton.playerMaxAmmo);
        UpdatePlayerHealth(player);
    }

    void UpdateWaveDisplay(float timeToDisplay) {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = "Wave: " + $"{GameManager.singleton.wave}\n" + $"{minutes:00}:{seconds:00}";
        enemiesText.text = $"{GameManager.singleton.transform.childCount}" + " Hostiles";
    }
}