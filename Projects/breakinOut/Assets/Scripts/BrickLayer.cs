using System;
using TMPro;
using UnityEngine;

public class BrickLayer : MonoBehaviour {
    public static BrickLayer singleton;
    public GameObject brick;
    public int rows, columns;
    public int numBricks;
    public float brickSpacingH, brickSpacingV;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        singleton = this;
    }

    void Start() {
        LayBricks();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void LayBricks() {
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                float xPos = this.transform.localPosition.x - columns + j * brickSpacingH;
                float yPos = this.transform.localPosition.y + rows - i * brickSpacingV;
                GameObject instance = Instantiate(brick, new Vector3(xPos, yPos, 0), transform.rotation, this.transform);
                BrickScript brickCode = instance.GetComponent<BrickScript>();
                brickCode.lives = 2;
            }
        }
    }
    // Update is called once per frame
    void Update() {
        if (this.transform.childCount == 0) GameManagement.singleton.NextLevel();
    }
}
