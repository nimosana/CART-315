using UnityEngine;

public class CameraFollow : MonoBehaviour {
    // Drone related
    public Transform target;
    public Rigidbody targetRb;

    // Camera related
    public float maxSpeed = 10f;
    public Vector3 minOffset = new Vector3(0, 5, -5);
    public Vector3 maxOffset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;

    private Vector3 _velocity = Vector3.zero;

    void LateUpdate() {
        if (!target || !targetRb) return;

        // Calculate dynamic offset based on speed
        Vector3 dynamicOffset =
            Vector3.Lerp(minOffset, maxOffset, Mathf.Clamp01(targetRb.linearVelocity.magnitude / maxSpeed));

        // Flatten rotation to apply offset without affecting Y-axis
        Vector3 desiredPosition = target.position + Quaternion.Euler(0, target.eulerAngles.y, 0) * dynamicOffset;

        // Smoothly move the camera and look at the target
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);
        transform.LookAt(target.position + Vector3.up);
    }

    public void SetTarget(GameObject player) {
        if (player) {
            target = player.transform;
            targetRb = player.GetComponent<Rigidbody>();
        }
        else {
            Debug.LogError("Player is null.");
        }
    }
}