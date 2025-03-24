using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public int wave = 10;
    public GameObject uiInstance;

    public GameObject EnemyDrone;
    public List<GameObject> enemies;
    public Vector2 enemySpawnX;
    public Vector2 enemySpawnY;
    private Minimap mapScript;
    public GameObject playerInstance;
    public GameObject playerPrefab;
    private DroneMovement playerScript;
    private Health playerHealthScript;
    public GameObject basePrefab;

    public Vector3 spawnPosition = new Vector3(0, 0, 0);
    public float playerMaxHealth = 100f;
    public float playerMaxAmmo = 500f;

    public float shakeDuration = 0.01f;
    public float shakeIntensity = 0.02f;
    public float decreaseFactor = 1.5f;

    public Camera cameraInstance;
    private CameraFollow cameraScript;
    public Vector3 CameraSpawn;
    private bool shaking = false;
    private float thisShakeDuration;
    private float remainingIntensity;

    void Awake() {
        singleton = this;
    }

    private void Start() {
        cameraInstance = Instantiate(cameraInstance, CameraSpawn, Quaternion.identity);
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        uiInstance = Instantiate(uiInstance, Vector3.zero, Quaternion.identity);

        playerScript = playerInstance.GetComponent<DroneMovement>();
        playerHealthScript = playerInstance.GetComponent<Health>();
        mapScript = uiInstance.transform.Find("Canvas/MinimapBackground").gameObject.GetComponent<Minimap>();
        cameraScript = cameraInstance.GetComponent<CameraFollow>();
        uiInstance.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        uiInstance.GetComponentInChildren<Canvas>().worldCamera = cameraInstance;
        uiInstance.GetComponentInChildren<Canvas>().planeDistance = 1.5f;
        playerScript.mainCamera = cameraInstance;
        playerScript.setUI(uiInstance);
        playerHealthScript.setUI(uiInstance);
        cameraScript.SetTarget(playerInstance);

        spawnWaves();
        mapScript.player = playerInstance.GetComponent<Transform>();
        mapScript.enemyDrones = enemies;
    }

    private void Update() {
        singleton.ManageShake();
    }

    public void spawnWaves() {
        for (int i = 0; i < wave * 2; i++) {
            float xPos = Random.Range(enemySpawnX.x, enemySpawnX.y);
            float zPos = Random.Range(enemySpawnY.x, enemySpawnY.y);
            GameObject instance =
                Instantiate(EnemyDrone, new Vector3(xPos, 76, zPos), Quaternion.identity, this.transform);
            EnemyDrone enemyAI = instance.GetComponent<EnemyDrone>();
            enemyAI.SetTarget(basePrefab.transform);
            enemies.Add(instance);
            mapScript.NewWave();
        }
    }

    public void ManageShake() {
        if (!shaking && shakeDuration > 0) {
            thisShakeDuration = shakeDuration;
        }

        if (shakeDuration > 0) {
            float remainingIntensity = shakeIntensity * (shakeDuration / thisShakeDuration);
            cameraInstance.transform.localPosition = cameraInstance.transform.localPosition + new Vector3(
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

    public void respawnPlayer() {
        StartCoroutine(respawnPlayer());

        IEnumerator respawnPlayer() {
            yield return new WaitForSeconds(3f);
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerScript = playerInstance.GetComponent<DroneMovement>();
            playerHealthScript = playerInstance.GetComponent<Health>();

            playerScript.mainCamera = cameraInstance;
            playerScript.setUI(uiInstance);
            playerHealthScript.setUI(uiInstance);
            cameraScript.SetTarget(playerInstance);
            mapScript.player = playerInstance.GetComponent<Transform>();

            singleton.shakeIntensity = 0.03f;
        }
    }
}