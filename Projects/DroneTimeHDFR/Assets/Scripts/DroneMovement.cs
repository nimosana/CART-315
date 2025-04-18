using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class DroneMovement : MonoBehaviour {
    public float accelForce = 10f; // Forward movement force
    public float rotationAccel = 0.2f; // Rotation speed
    public float maxSpeed = 10f; // Maximum movement speed
    public float maxRotationSpeed = 6f;
    public float drag = 1.5f;
    public float bulletSpawnDistance = .22f;
    public float tiltAmount = 10f; // Max degrees tilt angle 
    public float tiltSmoothing = 5f;
    public bool invincible = false;
    public GameObject projectilePrefab;
    public GameObject bulletShellPrefab;
    public float playerAmmo = 200;
    public float projectileSpeed = 50f;
    public float fireRate = 0.1f; // Time between shots
    private float nextFireTime;
    private Health healthScript;
    public VisualEffect explosionEffect;
    public float suicideForce = 500f;
    public float suicideRadius = 10f;
    public float suicideDamage = 200f;
    public KeyCode detonateKey = KeyCode.Space;
    public KeyCode rotateKey = KeyCode.LeftShift;

    public UIManager uiManager;
    private Rigidbody rb;
    private Vector3 previousVelocity;
    public Camera mainCamera;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = drag;
        rb.angularDamping = drag;
        previousVelocity = Vector3.zero;
        healthScript = GetComponent<Health>();
        playerAmmo = GameManager.singleton.playerMaxAmmo;
    }

    private void Update() {
        if (Input.GetKeyDown(detonateKey)) {
            SuicideDrone();
        }
    }

    void FixedUpdate() {
        if (!Input.GetKey(rotateKey)) {
            RotateTowardsMouse();
        }

        MoveDrone();
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && playerAmmo > 0) {
            playerAmmo--;
            uiManager.UpdateAmmo(playerAmmo);
            ShootProjectile();
            nextFireTime = Time.time + fireRate;
        }

        if (invincible) {
            Health health = gameObject.GetComponent<Health>();
            health.currentHealth = 100;
        }

        ApplyVisualTilt();
    }

    void MoveDrone() {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float currentY = rb.position.y;

        // Vertical movement (Shift = up, Ctrl = down)
        float verticalInput = 0f;

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentY > 75.9f) {
            verticalInput = -1f; // move up
        }
        else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && currentY < 100.1) {
            verticalInput = 1f; // move down
        }

        if (currentY < 75.9f) {
            verticalInput = -1f;
        } else if (currentY > 100) {
            verticalInput = 1f;
        }

        input.y = verticalInput;

        Vector3 force = transform.TransformDirection(input * accelForce);
        rb.AddForce(-force, ForceMode.Acceleration);

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

        Vector3 clampedPosition = rb.position;

        rb.position = clampedPosition;
    }


    void RotateTowardsMouse() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, 76, 0));

        if (groundPlane.Raycast(ray, out float distance)) {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - transform.position).WithY(0);


            if (direction.sqrMagnitude > 0.01f) {
                Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);

                // Calculate angular difference using shortest path
                Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
                deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

                if (angle > 180f) angle -= 360f;

                rb.AddTorque(axis.normalized * (angle * rotationAccel), ForceMode.Acceleration);
                rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxRotationSpeed);
            }
        }
    }

    void ApplyVisualTilt() {
        // Calculate local velocity and acceleration in local space.
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 localAcceleration =
            transform.InverseTransformDirection((rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime);
        previousVelocity = rb.linearVelocity;

        // Determine tilt factors based on speed and acceleration.
        float speedFactor = Mathf.Clamp01(localVelocity.magnitude / maxSpeed);
        float tiltForward = Mathf.Clamp((localVelocity.z * speedFactor + localAcceleration.z) * tiltAmount, -tiltAmount,
            tiltAmount);
        float tiltSideways = Mathf.Clamp((-localVelocity.x * speedFactor - localAcceleration.x) * tiltAmount,
            -tiltAmount, tiltAmount);

        // Build the target rotation by preserving current yaw and adding tilt on pitch and roll.
        float yaw = transform.eulerAngles.y;
        Quaternion baseYaw = Quaternion.Euler(0, yaw, 0);
        Quaternion tiltRotation = Quaternion.Euler(tiltForward, 0, tiltSideways);
        Quaternion targetRotation = baseYaw * tiltRotation;

        // Smoothly interpolate from the current rotation to the target rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmoothing);
    }

    void ShootProjectile() {
        // Set spawn positions for projectile and shell
        Vector3 spawnPosition = rb.position - transform.forward * bulletSpawnDistance;
        Vector3 shellPosition = rb.position - transform.right * 0.07f;

        // Instantiate projectile and shell
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        GameObject bulletShell = Instantiate(bulletShellPrefab, shellPosition,
            Quaternion.Euler(-90, transform.eulerAngles.y, 0));
        ShakeManager.Instance.AddShake(0.05f, 0.02f);
        // Configure projectile
        if (projectile.TryGetComponent(out Rigidbody projectileRb)) {
            projectileRb.position = new Vector3(projectileRb.position.x, 76f, projectileRb.position.z);
            Quaternion correctedRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            projectileRb.linearVelocity = rb.linearVelocity + correctedRotation * Vector3.back * projectileSpeed;
            projectileRb.transform.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, 0);
        }
        else {
            Debug.LogError("No Rigidbody on projectile!");
        }

        // Configure shell physics
        if (bulletShell.TryGetComponent(out Rigidbody shellRb)) {
            shellRb.linearVelocity = rb.linearVelocity;
            shellRb.AddForce(transform.right * Random.Range(-0.15f, -.3f), ForceMode.Impulse);
            shellRb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }

        // Clean up
        Destroy(bulletShell, 3f);
        Destroy(projectile, 7f);
    }

    public void SetUI(GameObject uiObject) {
        uiManager = uiObject.GetComponent<UIManager>();
    }

    public void SuicideDrone() {
        // Spawn the explosion effect
        VisualEffect effectInstance = Instantiate(explosionEffect, rb.position, Quaternion.identity);
        Destroy(effectInstance.gameObject, 5f);
        effectInstance.Play();
        ShakeManager.Instance.AddShake(1f, 0.25f);
        healthScript.exploded = true;

        // Apply damage to nearby enemies
        Collider[] colliders = Physics.OverlapSphere(rb.position, suicideRadius);
        if (colliders.Length > 0) {
            foreach (Collider nearbyObject in colliders) {
                if (nearbyObject.CompareTag("EnemyDrone")) {
                    Health enemyHealth = nearbyObject.GetComponent<Health>();
                    Rigidbody enemyRb = nearbyObject.GetComponent<Rigidbody>();
                    // Ensure both components exist before proceeding
                    if (!enemyHealth || !enemyRb)
                        continue;
                    float distance = Vector3.Distance(rb.position, enemyRb.position);
                    float damageFactor = Mathf.Clamp01(1 - (distance / suicideRadius));

                    enemyRb.AddExplosionForce(suicideForce, rb.position, suicideRadius);
                    StartCoroutine(DelayedDamage(enemyHealth, suicideDamage * damageFactor));
                    Debug.Log("DAMAGE: " + suicideDamage * damageFactor);
                }
            }

            StartCoroutine(DelayedDeath());
        }

        // Coroutine so the drone bodies yeet away
        IEnumerator DelayedDamage(Health enemyHealth, float damage) {
            yield return new WaitForSeconds(0.01f);
            if (enemyHealth) {
                enemyHealth.TakeDamage(damage);
            }
        }

        IEnumerator DelayedDeath() {
            yield return new WaitForSeconds(0.02f);
            healthScript.Die();
        }
    }
}