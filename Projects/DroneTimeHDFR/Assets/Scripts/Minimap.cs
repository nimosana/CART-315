using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {
    public Transform player;
    public List<GameObject> enemyDrones;
    public RectTransform playerIcon;
    public RectTransform enemyIcon;
    public RectTransform homeBaseIcon;
    private List<RectTransform> enemyIcons = new List<RectTransform>();

    public float mapScale = 0.7f;

    void Start() {
        // Instantiate icons for each enemy
        foreach (GameObject enemy in enemyDrones) {
            RectTransform newIcon = Instantiate(enemyIcon, enemyIcon.transform.parent);
            enemyIcons.Add(newIcon);
        }
    }

    void Update() {
        // Rotate the MinimapBackground based on the player's rotation
        if (player) {
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0,
                -180 + GameManager.singleton.playerInstance.transform.eulerAngles.y);
            UpdatePlayerIcon(GameManager.singleton.playerInstance.transform, playerIcon);
        }

        for (int i = enemyDrones.Count - 1; i >= 0; i--) {
            if (!enemyDrones[i]) {
                // Remove both the destroyed enemy and its icon
                Destroy(enemyIcons[i].gameObject);
                enemyIcons.RemoveAt(i);
                enemyDrones.RemoveAt(i);
            }
            else {
                UpdateIcon(enemyDrones[i].transform.position, enemyIcons[i]);
                enemyIcons[i].gameObject.SetActive(true);
            }
        }

        if (GameManager.singleton.baseInstance.transform) {
            UpdateIcon(GameManager.singleton.basePosition, homeBaseIcon);
        }
    }

    void UpdateIcon(Vector3 target, RectTransform icon) {
        if (!player) return;
        Vector3 offset = target - GameManager.singleton.playerInstance.transform.position;
        Vector2 minimapPos = new Vector2(offset.x * mapScale, offset.z * mapScale);

        // Clamp to keep within the minimap boundaries
        icon.anchoredPosition = Vector2.ClampMagnitude(minimapPos, GetComponent<RectTransform>().sizeDelta.x / 2);

        // Rotate icons to match their actual world rotation
        // icon.localRotation = Quaternion.Euler(0, 0, -target.eulerAngles.y);
    }

    void UpdatePlayerIcon(Transform target, RectTransform icon) {
        if (!player) return;
        Vector3 offset = target.position - GameManager.singleton.playerInstance.transform.position;
        Vector2 minimapPos = new Vector2(offset.x * mapScale, offset.z * mapScale);

        // Clamp to keep within the minimap boundaries
        icon.anchoredPosition = Vector2.ClampMagnitude(minimapPos, GetComponent<RectTransform>().sizeDelta.x / 2);

        // Rotate icons to match their actual world rotation
        icon.localRotation = Quaternion.Euler(0, 0, -target.eulerAngles.y);
    }

    public void NewWave() {
        // Remove any remaining old icons
        foreach (var icon in enemyIcons) {
            Destroy(icon.gameObject);
        }

        enemyIcons.Clear();

        foreach (GameObject enemy in enemyDrones) {
            RectTransform newIcon = Instantiate(enemyIcon, enemyIcon.transform.parent);
            enemyIcons.Add(newIcon);
        }
    }
}