using UnityEngine;

public class BrickScript : MonoBehaviour {
    public int lives;
    private Renderer brickRenderer; // Cache the renderer

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        lives = GameManagement.level;
        brickRenderer = GetComponent<Renderer>();
        UpdateTransparency();
    }

    // Update is called once per frame
    void Update() {
    }

    public void HitBrick(float velocity) {
        GameManagement.singleton.AddPoints(1 * velocity);
        lives--;
        if (lives < 1) {
            Destroy(gameObject);
            GameManagement.singleton.Beep(0.25f);
        }
        else {
            UpdateTransparency();
            GameManagement.singleton.Beep(0.5f);
        }
    }

    private void UpdateTransparency() {
        // Get the current color of the brick
        Color currentColor = brickRenderer.material.color;
        // Convert to HSV (Hue, Saturation, Value)
        Color.RGBToHSV(currentColor, out float h, out float s, out float v);
        // Calculate the reduction factor based on lives
        float factor = Mathf.Clamp01((float)lives / GameManagement.level);
        s = Mathf.Clamp01(s * factor); // Desaturate
        v = Mathf.Clamp01(v * factor); // Darken
        // Convert back to RGB and apply
        brickRenderer.material.color = Color.HSVToRGB(h, s, v);
    }
}