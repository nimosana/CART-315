using TMPro;
using UnityEngine;

public class Gemeover : MonoBehaviour {
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        text.text = "Game Over\n\nPoints: " +GameManagement.points.ToString("F2");
    }

    // Update is called once per frame
    void Update() {
        
    }
}
