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
    public UIManager uiScript;
    public GameObject enemyDrone;
    public List<GameObject> enemies;
    public Vector2 enemySpawnX;
    public Vector2 enemySpawnY;

    public GameObject playerInstance;
    public GameObject playerPrefab;
    private DroneMovement playerScript;
    private Health playerHealthScript;
    public Vector3 spawnPosition = new Vector3(0, 0, 0);
    public float playerMaxHealth = 100f;
    public float playerMaxAmmo = 500f;
    public bool playerAlive = true;
    public GameObject baseInstance;
    public Vector3 basePosition = new Vector3(502, 54.5f, 518f);
    private Minimap mapScript;

    public float shakeDuration = 0.01f;
    public float shakeIntensity = 0.02f;
    public float shakeBigIntensity = 0.02f;
    public float decreaseFactor = 1.5f;
    public Camera cameraInstance;
    private CameraFollow cameraScript;
    public Vector3 CameraSpawn;
    private float thisShakeDuration;
    private float remainingIntensity;
    public ShakeManager shakerInstance;

    void Awake() {
        singleton = this;
        cameraInstance = Instantiate(cameraInstance, CameraSpawn, Quaternion.identity);
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        uiInstance = Instantiate(uiInstance, Vector3.zero, Quaternion.identity);
        shakerInstance = Instantiate(shakerInstance, Vector3.zero, Quaternion.identity);
        // baseInstance.transform.position = basePosition;
        playerScript = playerInstance.GetComponent<DroneMovement>();
        playerHealthScript = playerInstance.GetComponent<Health>();
        mapScript = uiInstance.transform.Find("Canvas/MinimapBackground").gameObject.GetComponent<Minimap>();
        cameraScript = cameraInstance.GetComponent<CameraFollow>();
        uiInstance.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        uiInstance.GetComponentInChildren<Canvas>().worldCamera = cameraInstance;
        uiInstance.GetComponentInChildren<Canvas>().planeDistance = 1.5f;
        playerScript.mainCamera = cameraInstance;
        playerScript.SetUI(uiInstance);
        playerHealthScript.SetUI(uiInstance);
        uiScript = uiInstance.GetComponent<UIManager>();
        cameraScript.SetTarget(playerInstance);
        mapScript.player = playerInstance.GetComponent<Transform>();
        mapScript.enemyDrones = enemies;
    }

    private void Start() {
        SpawnWaves();
    }

    public void SpawnWaves() {
        for (int i = 0; i < wave * 2; i++) {
            float xPos = Random.Range(enemySpawnX.x, enemySpawnX.y);
            float zPos = Random.Range(enemySpawnY.x, enemySpawnY.y);
            GameObject instance =
                Instantiate(enemyDrone, new Vector3(xPos, 76, zPos), Quaternion.identity, this.transform);
            enemies.Add(instance);
            mapScript.NewWave();
        }
    }

    public void RespawnPlayer() {
        StartCoroutine(RespawnIEnumerator());

        IEnumerator RespawnIEnumerator() {
            yield return new WaitForSeconds(3f);
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerScript = playerInstance.GetComponent<DroneMovement>();
            playerHealthScript = playerInstance.GetComponent<Health>();
            playerScript.mainCamera = cameraInstance;
            playerScript.SetUI(uiInstance);
            playerHealthScript.SetUI(uiInstance);
            cameraScript.SetTarget(playerInstance);
            mapScript.player = playerInstance.GetComponent<Transform>();

            uiScript.NewDroneUI(playerInstance);
            playerAlive = true;
        }
    }
}