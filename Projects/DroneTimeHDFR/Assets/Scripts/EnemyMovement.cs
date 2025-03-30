using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

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
    public float liftForce = 10f;
    private float distanceToBase;
    public float distanceToPlayer;
    public float playerTargetDistance = 10f;
    private float nextFireTime;
    public VisualEffect explosionEffect;
    public float suicideForce = 500f;
    public float suicideRadius = 10f;
    public float suicideDamage = 200f;
    private Health healthScript;
    private Rigidbody rb;
    private Vector3 previousVelocity;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = drag;
        previousVelocity = Vector3.zero;
        healthScript = GetComponent<Health>();
    }

    void FixedUpdate() {
        Move();
        ApplyVisualTilt();
    }

    void Move() {
        if (GameManager.singleton.playerAlive) {
            distanceToPlayer = GetDistanceToPlayer(GameManager.singleton.playerInstance.transform);
        }

        distanceToBase = GetDistanceToBase(GameManager.singleton.basePosition);

        if (GameManager.singleton.playerAlive && distanceToPlayer < distanceToBase && distanceToPlayer <= 25) {
            TargetPlayer(distanceToPlayer);
        }
        else {
            if (distanceToBase < 33) {
                TargetHouse(GameManager.singleton.baseInstance.transform.position.y + 2f);
            }
            else {
                TargetHouse(76);
            }
        }
    }

    void TargetHouse(float houseHeight) {
        RotateTowardsTarget(GameManager.singleton.basePosition);
        Vector3 liftDirection = Vector3.up * (liftForce * Mathf.Sign(houseHeight - rb.position.y));

        Vector3 directionToBase = (GameManager.singleton.basePosition +
                                   -transform.position).WithY(0).normalized;
        rb.AddForce((directionToBase * accelForce) + liftDirection, ForceMode.Acceleration);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        if (distanceToBase < 7) {
            SuicideDrone();
        }
    }

    void TargetPlayer(float height) {
        RotateTowardsTarget(GameManager.singleton.playerInstance.transform.position);
        float directionSign = Mathf.Sign(height - playerTargetDistance);

        Vector3 directionToPlayer = (GameManager.singleton.playerInstance.transform.position - transform.position)
            .WithY(0).normalized;
        // Apply movement force and limit max speed
        Vector3 liftDirection = Vector3.up * (liftForce * Mathf.Sign(76 - rb.position.y));
        rb.AddForce(directionToPlayer * (directionSign * accelForce) + liftDirection, ForceMode.Acceleration);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

        if ((Time.time >= nextFireTime) && height < 15f) {
            ShootProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    void RotateTowardsTarget(Vector3 target) {
        Vector3 direction = (target - transform.position).WithY(0).normalized;

        if (direction.sqrMagnitude > 0.01f) {
            // Using sqrMagnitude for better performance
            Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0);
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);

            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
            axis = axis.normalized;

            if (angle > 180f) angle -= 360f;
            Vector3 torque = axis * (angle * rotationAccel) - rb.angularVelocity * 2f;
            rb.AddTorque(torque, ForceMode.Acceleration);

            // Limit max rotation speed
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxRotationSpeed);
        }
    }

    void ApplyVisualTilt() {
        // Calculate local space velocity and acceleration
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 localAcceleration =
            transform.InverseTransformDirection((rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime);
        previousVelocity = rb.linearVelocity;

        // Determine the tilt based on movement and acceleration
        float speedFactor = Mathf.Clamp01(localVelocity.magnitude / maxSpeed);
        float finalTiltForward = Mathf.Clamp((localVelocity.z * speedFactor + localAcceleration.z) * tiltAmount,
            -tiltAmount, tiltAmount);
        float finalTiltSideways = Mathf.Clamp((-localVelocity.x * speedFactor - localAcceleration.x) * tiltAmount,
            -tiltAmount, tiltAmount);

        Quaternion baseYawRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Quaternion tiltRotation = Quaternion.Euler(finalTiltForward, 0, finalTiltSideways);
        Quaternion finalRotation = baseYawRotation * tiltRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime * tiltSmoothing);
    }

    void ShootProjectile() {
        Vector3 spawnPosition = rb.position - transform.forward * bulletSpawnDistance;
        Vector3 shellPosition = rb.position - (transform.right * 0.07f) + Vector3.up * 0.1f;

        // Instantiate projectile and shell
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        GameObject bulletShell = Instantiate(bulletShellPrefab, shellPosition,
            Quaternion.Euler(-90, transform.eulerAngles.y, 0));

        // Apply physics to the shell
        Rigidbody shellRb = bulletShell.GetComponent<Rigidbody>();
        if (shellRb) {
            shellRb.linearVelocity = rb.linearVelocity;
            Vector3 sidewaysForce = transform.right * Random.Range(-0.5f, -1.5f);
            shellRb.AddForce(sidewaysForce + Vector3.up * 0.2f, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            shellRb.AddTorque(randomTorque, ForceMode.Impulse);
        }

        Destroy(bulletShell, 3f);

        // Apply physics to the projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb) {
            Quaternion correctedRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            projectileRb.linearVelocity = rb.linearVelocity + correctedRotation * Vector3.back * projectileSpeed;
            projectileRb.transform.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, 0);
            Destroy(projectile, 7f);
        }
    }

    public void SuicideDrone() {
        // Spawn the explosion effect
        VisualEffect effectInstance = Instantiate(explosionEffect, rb.position, Quaternion.identity);
        Destroy(effectInstance.gameObject, 5f);
        effectInstance.Play();
        ShakeManager.Instance.AddShake(1f, 0.1f);
        healthScript.exploded = true;

        // Apply damage to nearby enemies
        Collider[] colliders = Physics.OverlapSphere(rb.position, suicideRadius);
        if (colliders.Length > 0) {
            foreach (Collider nearbyObject in colliders) {
                if (nearbyObject.CompareTag("EnemyDrone") || nearbyObject.CompareTag("Player")) {
                    Health enemyHealth = nearbyObject.GetComponent<Health>();
                    Rigidbody enemyRb = nearbyObject.GetComponent<Rigidbody>();

                    if (!enemyHealth || !enemyRb)
                        continue;

                    float distance = Vector3.Distance(rb.position, enemyRb.position);
                    float damageFactor = Mathf.Clamp01(1 - (distance / suicideRadius));

                    enemyRb.AddExplosionForce(suicideForce, rb.position, suicideRadius);
                    StartCoroutine(DelayedDamage(enemyHealth, suicideDamage * damageFactor));
                }

                if (nearbyObject.CompareTag("Base")) {
                    Health baseHealth = nearbyObject.GetComponent<Health>();
                    float distance = Vector3.Distance(rb.position, GameManager.singleton.basePosition);
                    float damageFactor = Mathf.Clamp01(1 - (distance / suicideRadius));
                    baseHealth.BaseDamage(suicideDamage * damageFactor);
                    GameManager.singleton.uiInstance.GetComponent<UIManager>().UpdateBaseHealth();
                    Debug.Log("DAMAGE TO BASE: " + (suicideDamage * damageFactor));
                }
            }

            StartCoroutine(DelayedDeath());
        }

        // Coroutine to delay damage application
        IEnumerator DelayedDamage(Health enemyHealth, float damage) {
            yield return new WaitForSeconds(0.01f);
            if (enemyHealth) {
                enemyHealth.TakeDamage(damage);
            }
        }

        // Coroutine to delay death so explosion forces can be applied
        IEnumerator DelayedDeath() {
            yield return new WaitForSeconds(0.02f);
            if (healthScript) {
                healthScript.Die();
            }
        }
    }

    private float GetDistanceToPlayer(Transform playerTransform) =>
        Vector3.Distance(playerTransform.position, transform.position);

    private float GetDistanceToBase(Vector3 housePos) =>
        Vector3.Distance(housePos, transform.position);
}