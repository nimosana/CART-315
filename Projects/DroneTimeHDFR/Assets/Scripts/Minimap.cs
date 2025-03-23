using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour {
    public Transform player; // Player's Transform
    public List<GameObject> enemyDrones; // List of enemy drones
    public Transform homeBase; // Homebase Transform

    public RectTransform playerIcon; // UI Icon for the player
    public RectTransform enemyIcon; // Template UI Icon for enemies
    public RectTransform homeBaseIcon; // UI Icon for homebase

    private List<RectTransform> enemyIcons = new List<RectTransform>(); // List to store enemy icons

    public float mapScale = 1f; // Adjust to fit the map to the UI

    void Start() {
        // Instantiate icons for each enemy
        foreach (GameObject enemy in enemyDrones) {
            RectTransform newIcon = Instantiate(enemyIcon, enemyIcon.transform.parent);
            enemyIcons.Add(newIcon);
        }

        // enemyIcon.gameObject.SetActive(false); // Disable the template icon
    }

    void Update() {
        // Rotate the MinimapBackground based on the player's rotation
        if (player) {
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -180 + player.eulerAngles.y);
            UpdateIcon(player, playerIcon);
        }

        for (int i = enemyDrones.Count - 1; i >= 0; i--) {
            if (enemyDrones[i] == null) {
                // Remove both the destroyed enemy and its icon
                Destroy(enemyIcons[i].gameObject);
                enemyIcons.RemoveAt(i);
                enemyDrones.RemoveAt(i);
            }
            else {
                UpdateIcon(enemyDrones[i].transform, enemyIcons[i]);
                enemyIcons[i].gameObject.SetActive(true);
            }
        }

        if (homeBase != null) {
            UpdateIcon(homeBase, homeBaseIcon);
        }
    }

    void UpdateIcon(Transform target, RectTransform icon) {
        if (!player) return; 
        Vector3 offset = target.position - player.position;
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