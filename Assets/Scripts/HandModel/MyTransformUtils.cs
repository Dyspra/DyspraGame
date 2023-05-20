using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTransformUtils : MonoBehaviour
{
    public static float NormalizeAngle(float angle)
    {
        while (angle < -180) angle += 360;
        while (angle > 180) angle -= 360;
        return angle;
    }

    public static float UnNormalizeAngle(float angle)
    {
        while (angle < 0) angle += 360;
        return angle;
    }

    public static Vector3 GetInspectorRotation(Transform transform)
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        return new Vector3(NormalizeAngle(rotation.x), NormalizeAngle(rotation.y), NormalizeAngle(rotation.z));
    }

    public static void SetInspectorRotation(Transform transform, Vector3 eulerAngles)
    {
        // Un-normalize angles
        Vector3 unNormalizedEulerAngles = new Vector3(UnNormalizeAngle(eulerAngles.x), UnNormalizeAngle(eulerAngles.y), UnNormalizeAngle(eulerAngles.z));

        // Set the local rotation
        transform.localRotation = Quaternion.Euler(unNormalizedEulerAngles);
    }
}
