using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour {
    public static float points = 0;
    public int lives = 3;
    public static GameManagement singleton;
    public TextMeshProUGUI scoring;
    void Awake() {
        singleton = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
    }

    public void LoseLife() {
        lives--;
        Debug.Log(lives);
        if (lives <= 0) {
            GameOver();
        }
    }
    public void GameOver() {
        SceneManager.LoadScene("GameOver");
    }
    public void AddPoints(float goated) {
        points += goated;
        scoring.text = "Points:\n" + points.ToString("F2");
    }
    // Update is called once per frame
    void Update() {
    }
}