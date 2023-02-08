using UnityEngine;
using System.Collections.Generic;

public class CollisionDetection : MonoBehaviour
{
    public HapticDeviceManager hapticDeviceManager;
    private string _collisionObjectName;
    private Vector3 _collisionPoint;
    private float _collisionIntensity;

    private void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white, 10f);
        }
        Debug.Log("Collision detected with " + collision.gameObject.name + " with intensity " + collision.impulse.magnitude);
        // _collisionObjectName = collision.gameObject.name;
        // _collisionPoint = collision.contacts[0].point;
        // _collisionIntensity = collision.impulse.magnitude;

        HapticDeviceManager.Instance.SendHapticData(collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision ended with " + collision.gameObject.name);

        HapticDeviceManager.Instance.SendHapticData(_collisionObjectName);
    }

    private void OnCollisionStay(Collision collision)
    {
        _collisionObjectName = collision.gameObject.name;
        _collisionPoint = collision.contacts[0].point;
        _collisionIntensity = collision.impulse.magnitude;
        Debug.Log("Collision ongoing with " + _collisionObjectName + " at " + _collisionPoint + " with intensity " + _collisionIntensity);

        HapticDeviceManager.Instance.SendHapticData(_collisionObjectName);
    }
}
