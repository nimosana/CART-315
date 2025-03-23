using UnityEngine;

public class EnemyDrone : MonoBehaviour {
    public float accelForce = 10f;
    public float rotationAccel = 0.2f;
    public float maxSpeed = 10f;
    public float maxRotationSpeed = 2f;
    public float drag = 1.5f;
    public float bulletSpawnDistance = .22f;
    public float tiltAmount = 10f;
    public float tiltSmoothing = 5f;

    public GameObject projectilePrefab;
    public GameObject bulletShellPrefab;
    public float projectileSpeed = 50f;
    public float fireRate = 0.1f;
    private float nextFireTime;


    public Transform player;
    private Rigidbody rb;
    private Vector3 previousVelocity;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = drag;
        previousVelocity = Vector3.zero;
    }

    void FixedUpdate() {
        if (!player) return;
        RotateTowardsPlayer();
        MoveDrone();
        if (Time.time >= nextFireTime) {
            // ShootProjectile();
            nextFireTime = Time.time + fireRate;
        }
        ApplyVisualTilt();
    }

    void MoveDrone() {
        Vector3 directionToPlayer = (player.position - transform.position).WithY(0).normalized;

        // Apply movement force and limit max speed
        rb.AddForce(directionToPlayer * accelForce, ForceMode.Acceleration);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    void RotateTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).WithY(0).normalized;

        if (direction.sqrMagnitude > 0.01f) {
            // Using sqrMagnitude for better performance
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);

            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
            axis = axis.normalized;

            if (angle > 180f) angle -= 360f;

            // Apply proportional and derivative control
            Vector3 torque = axis * (angle * rotationAccel) - rb.angularVelocity * 2f;

            rb.AddTorque(torque, ForceMode.Acceleration);

            // Limit max rotation speed
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxRotationSpeed);
        }

        // Ensure it stays at the correct height
        rb.position = rb.position.WithY(76f);
    }

    void ApplyVisualTilt() {
        // Calculate local space velocity and acceleration
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 localAcceleration =
            transform.InverseTransformDirection((rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime);
        previousVelocity = rb.linearVelocity;

        // Determine the tilt based on movement and acceleration
        float speedFactor = Mathf.Clamp01(localVelocity.magnitude / maxSpeed); // Mathf.Clamp01 for cleaner clamping
        float finalTiltForward = Mathf.Clamp((localVelocity.z * speedFactor + localAcceleration.z) * tiltAmount,
            -tiltAmount, tiltAmount);
        float finalTiltSideways = Mathf.Clamp((-localVelocity.x * speedFactor - localAcceleration.x) * tiltAmount,
            -tiltAmount, tiltAmount);

        // Extract yaw and apply tilt using quaternions
        Quaternion baseYawRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion tiltRotation = Quaternion.Euler(finalTiltForward, 0, finalTiltSideways);
        Quaternion finalRotation = baseYawRotation * tiltRotation;

        // Smoothly interpolate towards the tilt using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * tiltSmoothing);
    }

    void ShootProjectile() {
        // Calculate spawn positions
        Vector3 spawnPosition = rb.position - transform.forward * bulletSpawnDistance;
        Vector3 shellPosition = rb.position - (transform.right * 0.07f) + Vector3.up * 0.1f;

        // Instantiate projectile and shell
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        GameObject bulletShell = Instantiate(bulletShellPrefab, shellPosition,
            Quaternion.Euler(-90, transform.eulerAngles.y, 0));

        // Apply physics to the shell
        Rigidbody shellRb = bulletShell.GetComponent<Rigidbody>();
        if (shellRb != null) {
            shellRb.linearVelocity = rb.linearVelocity;
            Vector3 sidewaysForce = transform.right * Random.Range(-0.5f, -1.5f);
            shellRb.AddForce(sidewaysForce + Vector3.up * 0.2f, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            shellRb.AddTorque(randomTorque, ForceMode.Impulse);
        }

        Destroy(bulletShell, 3f);

        // Apply physics to the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null) {
            Quaternion correctedRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            projectileRb.linearVelocity = correctedRotation * Vector3.back * projectileSpeed;
            projectileRb.transform.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, 0);
            Destroy(projectile, 7f);
        }
    }

    public void SetTarget(Transform playerTransform) {
        if (playerTransform != null) {
            player = playerTransform;
        }
        else {
            Debug.LogError("Player transform is null.");
        }
    }
}