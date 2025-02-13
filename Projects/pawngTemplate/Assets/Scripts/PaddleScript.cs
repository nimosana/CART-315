using UnityEngine;

public class PaddleScript : MonoBehaviour {
    public KeyCode upKey, downKey, leftKey, rightKey;
    private readonly float paddleAccel = 1800;
    private readonly float paddleAccelRot = 800;
    private readonly float paddleDamping = 2000;

    private Rigidbody2D rb;
    private float rotation, rotationAccel;

    // Start is called before the first frame update
    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKey(leftKey) && !Input.GetKey(rightKey) && Mathf.Abs(rotationAccel) < 0.05f) {
            rb.AddTorque(paddleAccelRot * Time.deltaTime);
        }
        else if (Input.GetKey(rightKey) && !Input.GetKey(leftKey) && Mathf.Abs(rotationAccel) < 0.05f) {
            rb.AddTorque(-paddleAccelRot * Time.deltaTime);
        }
        else {
            if (Mathf.Abs(rb.rotation) > 1) {
                rb.AddTorque(-Mathf.Sign(rb.rotation) * 25 * Time.deltaTime);
                rb.angularDamping = paddleDamping * Time.deltaTime;
            }
            else {
                rb.angularDamping = paddleDamping * 5 * Time.deltaTime;
            }
        }

        if (Input.GetKey(downKey) && !Input.GetKey(upKey))
            rb.AddForce(transform.up * (-paddleAccel * Time.deltaTime));
        else if (Input.GetKey(upKey) && !Input.GetKey(downKey))
            rb.AddForce(transform.up * (paddleAccel * Time.deltaTime));
        else rb.linearDamping = paddleDamping * Time.deltaTime;
    }
}