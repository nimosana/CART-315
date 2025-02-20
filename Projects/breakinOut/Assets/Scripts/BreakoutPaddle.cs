using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutPaddle : MonoBehaviour {
    private readonly float paddleAccel = 100;
    private readonly float paddleAccelRot = 30;
    private readonly float paddleDamping = 10;
    public KeyCode leftKey, rightKey, spaceKey;
    private float rotation, rotationAccel;
    private Rigidbody2D rb;
   
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(leftKey) && !Input.GetKey(rightKey)) {
            rb.AddForce(transform.up * (paddleAccel ));
            if (!Input.GetKey(spaceKey)) rb.AddTorque(paddleAccelRot );
            rb.linearDamping = 0;
        } else if (Input.GetKey(rightKey) && !Input.GetKey(leftKey)) {
            rb.AddForce(transform.up * (-paddleAccel ));
            if (!Input.GetKey(spaceKey)) rb.AddTorque(-paddleAccelRot );
            rb.linearDamping = 0;
        } else {
         rb.linearDamping = paddleDamping/3 ;
        }
        if ((Mathf.Abs(rb.rotation-90) > 1)&& !Input.GetKey(spaceKey)) {
            rb.AddTorque(-Mathf.Sign(rb.rotation-90) * (paddleAccelRot/3) );
        }
        rb.angularDamping = paddleDamping ;

        if (Mathf.Abs(rb.rotation - 90) > 45) {
            rb.rotation = Mathf.Sign(rb.rotation - 90)*45 +90;
            rb.angularVelocity *= -0.4f;
        }
    }
}

