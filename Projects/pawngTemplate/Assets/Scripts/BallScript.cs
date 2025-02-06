using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour {
    public Camera cam;
    public AudioSource blip;
    public float ballSpeed = 2;
    public int leftPlayerScore, rightPlayerScore;
    public Text scoreLeft, scoreRight; // Assign in the Inspector
    public float decreaseFactor = 1.5f; // Controls how quickly the shake stops
    private readonly int[] directions = { -1, 1 };
    private int hDir, vDir;
    private Vector3 lastVelocity; // Stores the velocity of the previous frame

    private Rigidbody2D rb;
    private float shakeDuration, intensity;

    private void Reset() {
        rb.linearVelocity = Vector2.zero;
        transform.localPosition = new Vector3(0, 0, 0);
        lastVelocity = rb.linearVelocity;
        scoreLeft.text = "Score\n" + leftPlayerScore;
        scoreRight.text = "Score\n" + rightPlayerScore;
        StartCoroutine(Launch());
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(Launch());
    }

    // Update is called once per frame
    private void Update() {
        {
            var originalPosition = transform.localPosition;
            if (shakeDuration > 0) {
                // Apply random shake displacement
                cam.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), -10);
                // Decrease shake duration
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else {
                // Reset to the original position
                shakeDuration = 0f;
                cam.transform.localPosition = new Vector3(0, 0, -10);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D wall) {
        Debug.Log("collided");
        shakeDuration = 0.25f;
        if (wall.gameObject.name == "leftWall") {
            rightPlayerScore += 1;
            blip.pitch = 0.15f;
            blip.Play();
            Reset();
        }
        else if (wall.gameObject.name == "rightWall") {
            leftPlayerScore += 1;
            blip.pitch = 0.15f;
            blip.Play();
            Reset();
        }
        else {
            blip.pitch = 1.25f;
            blip.Play();
        }

        if (wall.gameObject.name == "topWall" || wall.gameObject.name == "bottomWall") {
            blip.pitch = 0.75f;
            blip.Play();
        }
    }

    private IEnumerator Launch() {
        // choose Random X dir
        hDir = directions[Random.Range(0, directions.Length)];
        // choose random Y dir
        vDir = Random.Range(-2, 2);
        // wait for x secs
        yield return new WaitForSeconds(1);
        rb.AddForce(transform.right * ballSpeed * hDir);
        rb.AddForce(transform.up * ballSpeed * vDir);
    }
}