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

        // hapticDeviceManager.SendHapticCommand(_collisionObjectName);
    }

    private void OnCollisionExit(Collision collision)
    {
        _collisionObjectName = collision.gameObject.name;
        _collisionPoint = collision.contacts[0].point;
        _collisionIntensity = collision.impulse.magnitude;
        Debug.Log("Collision ended with " + _collisionObjectName + " at " + _collisionPoint + " with intensity " + _collisionIntensity);

        hapticDeviceManager.SendHapticCommand(_collisionObjectName);
    }

    private void OnCollisionStay(Collision collision)
    {
        _collisionObjectName = collision.gameObject.name;
        _collisionPoint = collision.contacts[0].point;
        _collisionIntensity = collision.impulse.magnitude;
        Debug.Log("Collision ongoing with " + _collisionObjectName + " at " + _collisionPoint + " with intensity " + _collisionIntensity);

        hapticDeviceManager.SendHapticCommand(_collisionObjectName);
    }
}
