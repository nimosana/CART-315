using UnityEngine;

public static class HelperStuff {
    public static Vector3 WithY(this Vector3 vector, float newY) {
        return new Vector3(vector.x, newY, vector.z);
    }
}