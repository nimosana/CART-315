using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakoutBall_working : MonoBehaviour
{
    private Rigidbody2D rb;
    public float ballSpeed = 20;
    public float maxSpeed = 100f;
    public float minSpeed = 20f;
    public Camera cam;
    public float decreaseFactor = 1.5f; // Controls how quickly the shake stops
    public AudioSource scoreSound, blip;
    
    private int[] dirOptions = {-4, 4};
    private int   hDir;

    private bool gameRunning;
    private float shakeDuration, intensity;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        Reset(); 
    }
    
    void Update() {
        if (!gameRunning) StartCoroutine(Launch());
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


    // Start the Ball Moving
    private IEnumerator Launch() {
        gameRunning = true;
        //yield return new WaitForSeconds(1.5f);
        
        // Figure out directions
        hDir = dirOptions[Random.Range(0, dirOptions.Length)];
        
        // Add a horizontal force
        rb.AddForce(transform.right * ballSpeed * hDir); // Randomly go Left or Right
        // Add a vertical force
        rb.AddForce(transform.up * ballSpeed* 8);
        
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
        shakeDuration = 0.25f;
        // did we hit a wall?
        if (other.gameObject.tag == "Wall") {
            // make pitch lower
            blip.pitch = 0.75f;
            blip.Play();
            SpeedCheck();
        }
        // did we hit a paddle?
        if (other.gameObject.tag == "Paddle") {
            // make pitch higher
            blip.pitch = 1f;
            blip.Play();
            SpeedCheck();
        }
        // did we hit the Bottom
        if (other.gameObject.tag == "Reset") {
            GameManagement.singleton.LoseLife();
            Reset();
        }
        if (other.gameObject.tag == "Brick") {
            GameManagement.singleton.AddPoints(1 * rb.linearVelocity.magnitude);
            Destroy(other.gameObject);
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
        // Debug.Log(rb.linearVelocity);
    }
}
