using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public GameObject EnemyDrone;
    public List<GameObject> enemies;
    public int wave;
    public float playerMaxHealth = 100f;
    public float playerMaxAmmo = 500f;

    public float shakeDuration = 0.01f;
    public float shakeIntensity = 0.02f;
    public float decreaseFactor = 1.5f;

    public Camera cam;
    private bool shaking = false;
    private float thisShakeDuration;
    private float remainingIntensity;

    void Awake() {
        singleton = this;
    }

    private void Start() {
        spawnWaves(10);
    }

    private void Update() {
        singleton.ManageShake();
    }

    public void spawnWaves(int wave) {
        for (int i = 0; i < wave * 2; i++) {
            float xPos = Random.Range(-1300, -1500);
            float zPos = Random.Range(-2200, -2400);
            GameObject instance =
                Instantiate(EnemyDrone, new Vector3(xPos, 76, zPos), Quaternion.identity, this.transform);
            enemies.Add(instance);
        }
    }

    public void ManageShake() {
        if (!shaking && shakeDuration > 0) {
            thisShakeDuration = shakeDuration;
        }

        if (shakeDuration > 0) {
            float remainingIntensity = shakeIntensity * (shakeDuration / thisShakeDuration);
            cam.transform.localPosition = cam.transform.localPosition + new Vector3(
                Random.Range(-remainingIntensity, remainingIntensity),
                Random.Range(-remainingIntensity, remainingIntensity), 0);
            shakeDuration -= Time.deltaTime * decreaseFactor;
            shaking = true;
        }
        else if (shaking) {
            shakeDuration = 0f;
            shaking = false;
        }
    }
}