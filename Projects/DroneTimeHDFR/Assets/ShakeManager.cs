using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeManager : MonoBehaviour {
    public static ShakeManager Instance;
    public Camera cameraInstance;
    private List<ShakeInstance> shakes = new List<ShakeInstance>();

    public float decreaseFactor = 1f;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        cameraInstance = GameManager.singleton.cameraInstance;
    }

    // add a new shake instance.
    public void AddShake(float duration, float intensity) {
        shakes.Add(new ShakeInstance(duration, intensity));
    }

    private void FixedUpdate() {
        if (shakes.Count == 0 || !cameraInstance)
            return;

        Vector3 totalOffset = Vector3.zero;

        for (int i = shakes.Count - 1; i >= 0; i--) {
            ShakeInstance shake = shakes[i];
            // Calculate remaining intensity based on time left
            float remainingIntensity = shake.intensity * (shake.remainingDuration / shake.duration);
            Vector3 offset = new Vector3(Random.Range(-remainingIntensity, remainingIntensity),
                Random.Range(-remainingIntensity, remainingIntensity),
                0);
            totalOffset += offset;

            shake.Update(Time.fixedDeltaTime * decreaseFactor);

            if (shake.remainingDuration <= 0) {
                shakes.RemoveAt(i);
            }
        }

        cameraInstance.transform.localPosition += totalOffset;
    }
}

public class ShakeInstance {
    public float duration;
    public float intensity;
    public float remainingDuration;

    public ShakeInstance(float duration, float intensity) {
        this.duration = duration;
        this.intensity = intensity;
        this.remainingDuration = duration;
    }

    public void Update(float deltaTime) {
        remainingDuration -= deltaTime;
    }
}