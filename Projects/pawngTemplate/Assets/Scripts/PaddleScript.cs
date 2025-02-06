using UnityEngine;

public class PaddleScript : MonoBehaviour {
    public float paddleAccel, paddleSpeed;
    public KeyCode upKey, downKey, leftKey, rightKey;
    public float bottomWall, topWall;

    private Rigidbody2D rb;
    private float rotation, rotationAccel;

    private float yPos;

    // Start is called before the first frame update
    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKey(leftKey) && !Input.GetKey(rightKey) && Mathf.Abs(rotationAccel) < 0.05f) {
            Debug.Log(rotationAccel);
            rotationAccel += 0.1f * Time.deltaTime;
        }
        else if (Input.GetKey(rightKey) && !Input.GetKey(leftKey) && Mathf.Abs(rotationAccel) < 0.05f) {
            Debug.Log(rotationAccel);
            rotationAccel += -0.1f * Time.deltaTime;
        }
        else if ((!Input.GetKey(upKey) && !Input.GetKey(leftKey)) || (Input.GetKey(upKey) && Input.GetKey(leftKey))) {
            rotationAccel /= 1.005f;
        }

        if (rb.position.y < bottomWall) {
            yPos = bottomWall;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
        }
        else if (rb.position.y > topWall) {
            yPos = topWall;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -rb.linearVelocity.y);
        }

        Debug.Log("pos " + rb.position.y);
        if (Input.GetKey(downKey) && !Input.GetKey(upKey))
            rb.AddForce(transform.up * -800 * Time.deltaTime);
        else if (Input.GetKey(upKey) && !Input.GetKey(downKey) && rb.position.y < topWall)
            rb.AddForce(transform.up * 800 * Time.deltaTime);
        else
            yPos += paddleAccel * Time.deltaTime;
        rotation += rotationAccel;
        if (Mathf.Abs(rotation) > 4.5f) {
            rotation = 4.5f * Mathf.Sign(rotation);
            rotationAccel *= -0.5f;
        }

        // transform.localPosition = new Vector3(transform.position.x, yPos, 0);
        transform.eulerAngles = new Vector3(0, 0, rotation * 10);
    }
}