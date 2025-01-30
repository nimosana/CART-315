using UnityEngine;
using System;
using System.Threading;

public class Collector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Rigidbody2D rb;
    public float xLoc, yLoc = 0;
    public float speedX, speedY = 0;
    public float accel = 1.0f;
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            // rb.AddForce(Vector2.right * accel, ForceMode2D.Force);
            speedX -= (accel * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            // rb.AddForce(Vector2.left * accel, ForceMode2D.Force);
            speedX += (accel * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            // rb.AddForce(Vector2.right * accel, ForceMode2D.Force);
            speedY += (accel * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            // rb.AddForce(Vector2.left * accel, ForceMode2D.Force);
            speedY -= (accel * Time.deltaTime);
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            speedX = 0;
        }
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            speedY = 0;
        }
        Debug.Log("speed " + speedX + " accel " + (accel * Time.deltaTime));
        xLoc += speedX;
        yLoc += speedY;
        this.transform.position = new Vector3(xLoc, yLoc, 0);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Circle")
        {
            Debug.Log("PeenD");
            Destroy(other.gameObject);
        }
    }
}
