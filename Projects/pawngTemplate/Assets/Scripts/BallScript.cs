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
    private readonly int[] directions = { -4, 4 };
    private bool ballReset;
    private int hDir, vDir;
    private int nextServe;

    private Rigidbody2D rb;
    private float shakeDuration, intensity;

    private void Reset() {
        transform.localPosition = new Vector2(0, 0);
        rb.linearVelocity = Vector2.zero;
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
            if (Mathf.Abs(rb.position.y) > 8 && !ballReset) {
                ballReset = true;
                nextServe = 0;
                Debug.Log("Reset");
                Reset();
            }

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
        shakeDuration = 0.25f;
        if (wall.gameObject.name == "leftWall") {
            rightPlayerScore += 1;
            blip.pitch = 0.15f;
            nextServe = -4;
            blip.Play();
            Reset();
        }
        else if (wall.gameObject.name == "rightWall") {
            leftPlayerScore += 1;
            blip.pitch = 0.15f;
            nextServe = 4;
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
        transform.localPosition = new Vector2(0, 0);
        // choose Random X dir
        if (nextServe == 0)
            hDir = directions[Random.Range(0, directions.Length)];
        else
            hDir = nextServe;
        // choose random Y dir
        vDir = Random.Range(-4, 4);
        // wait for x secs
        yield return new WaitForSeconds(1);
        ballReset = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(transform.right * (ballSpeed * hDir));
        rb.AddForce(transform.up * (ballSpeed * vDir));
    }
}