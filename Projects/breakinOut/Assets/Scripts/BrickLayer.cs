using TMPro;
using UnityEngine;

public class BrickLayer : MonoBehaviour {
    public GameObject brick;
    public GameObject ball;
    private Rigidbody2D ballBody;
    public int rows, columns;
    private int level = 1;
    public TextMeshProUGUI levelText;
    public int numBricks;
    public float brickSpacingH, brickSpacingV;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        ballBody = ball.GetComponent<Rigidbody2D>();
        ballBody.sharedMaterial.bounciness = 1.05f;
        LayBricks();
    }

    private void LayBricks() {
        for (int i = 0; i < columns; i++) {
            for (int j = 0; j < rows; j++) {
                float xPos = this.transform.localPosition.x - columns + j * brickSpacingH;
                float yPos = this.transform.localPosition.y + rows - i * brickSpacingV;
                Instantiate(brick, new Vector3(xPos, yPos, 0), transform.rotation, this.transform);
                // Debug.Log(xPos + ", " + yPos);
            }
        }
    }
    // Update is called once per frame
    void Update() {
        if (this.transform.childCount == 0) {
            LayBricks();
            level++;
            levelText.text = "Level:\n" + level;
            ballBody.sharedMaterial.bounciness += 0.05f;
        }
        // Debug.Log("bounciness: " + ballBody.sharedMaterial.bounciness);
    }
}
