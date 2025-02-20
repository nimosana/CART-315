using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakoutBall_working : MonoBehaviour {
    private Rigidbody2D rb;
    public float ballSpeed = 20;
    public float maxSpeed = 100f;
    public float minSpeed = 20f;
    public AudioSource scoreSound, blip;

    private readonly int[] dirOptions = { -4, 4 };
    private int hDir;

    private bool gameRunning;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Reset();
    }

    void Update() {
        if (!gameRunning) StartCoroutine(Launch());
    }


    // Start the Ball Moving
    private IEnumerator Launch() {
        gameRunning = true;
        // Figure out directions
        hDir = dirOptions[Random.Range(0, dirOptions.Length)];
        // Add a horizontal force
        rb.AddForce(transform.right * (ballSpeed * hDir)); // Randomly go Left or Right
        // Add a vertical force
        rb.AddForce(transform.up * (ballSpeed * 8));

        yield return null;
    }

    public void Reset() {
        rb.linearVelocity = Vector2.zero;
        ballSpeed = 20;
        transform.position = new Vector2(0, -2.4f);
        gameRunning = false;
    }

    // if the ball goes out of bounds
    private void OnCollisionEnter2D(Collision2D other) {
        GameManagement.singleton.shakeDuration = 0.25f;
        // did we hit a wall?
        if (other.gameObject.CompareTag("Wall")) {
            // make pitch lower
            GameManagement.singleton.Beep(0.75f);
            SpeedCheck();
        }

        // did we hit a paddle?
        if (other.gameObject.CompareTag("Paddle")) {
            // make pitch higher
            GameManagement.singleton.Beep(1f);
            SpeedCheck();
        }

        // did we hit the Bottom
        if (other.gameObject.CompareTag("Reset")) {
            GameManagement.singleton.LoseLife();
            Reset();
            GameManagement.singleton.Beep(0.1f);
        }

        if (other.gameObject.CompareTag("Brick")) {
            BrickScript brickCode = other.gameObject.GetComponent<BrickScript>();
            brickCode.HitBrick(rb.linearVelocity.magnitude);
        }
    }

    private void SpeedCheck() {
        // Prevent too shallow of an angle
        if (Mathf.Abs(rb.linearVelocity.x) < minSpeed) {
            // shorthand to check for existing direction
            rb.linearVelocity = new Vector2((rb.linearVelocity.x < 0) ? -minSpeed : minSpeed, rb.linearVelocity.y);
        }

        if (Mathf.Abs(rb.linearVelocity.y) < minSpeed) {
            // shorthand to check for existing direction
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, (rb.linearVelocity.y < 0) ? -minSpeed : minSpeed);
        }
    }
}