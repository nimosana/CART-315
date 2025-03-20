using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManagement : MonoBehaviour {
    public static float points = 0;
    public int lives = 3;
    public GameObject ball;
    private Rigidbody2D ballBody;
    public static GameManagement singleton;
    public static int level = 3;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI scoring;
    public TextMeshProUGUI livesText;
    public Camera cam;
    public float decreaseFactor = 1.5f; // Controls how quickly the shake stops
    public float shakeDuration = 0;
    public AudioSource scoreSound, blip;

    private readonly float intensity = 0.1f;
    void Awake() {
        singleton = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        ballBody = ball.GetComponent<Rigidbody2D>();
        ballBody.sharedMaterial.bounciness = 1 + 0.05f * level;
        levelText.text = "Level:\n" + level;
    }
    void Update() {
        singleton.ManageShake();
    }

    public void Beep(float tone) {
        blip.pitch = tone;
        blip.Play();
    }
    public void ManageShake() {
        if (shakeDuration > 0) {
            // Apply random shake displacement
            cam.transform.localPosition = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), -10);
            shakeDuration -= Time.deltaTime * decreaseFactor;
        } else {
            // Reset to the original position
            shakeDuration = 0f;
            cam.transform.localPosition = new Vector3(0, 0, -10);
        }
    }
    public void LoseLife() {
        lives--;
        if (lives <= 0) GameOver();
        livesText.text = "Lives: " + lives;
    }
    private void GameOver() {
        SceneManager.LoadScene("GameOver");
    }
    public void AddPoints(float addedPoints) {
        points += addedPoints;
        scoring.text = "Points:\n" + points.ToString("F2");
    }

    public void NextLevel() {
        BrickLayer.singleton.LayBricks();
        level++;
        levelText.text = "Level:\n" + GameManagement.level;
        ballBody.sharedMaterial.bounciness += 0.05f;
        if (lives < 3) lives++;
    }
}