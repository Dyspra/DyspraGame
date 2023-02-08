using UnityEngine;
using System.Collections.Generic;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white, 10f);
        }
        Debug.Log("Collision detected with " + collision.gameObject.name + " with intensity " + collision.impulse.magnitude);

        HapticDeviceManager.Instance.SendHapticData(collision.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision ended with " + collision.gameObject.name);

        HapticDeviceManager.Instance.SendHapticData(collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.name + " is still happening");

        HapticDeviceManager.Instance.SendHapticData(collision.gameObject.name);
    }
}
