using UnityEngine;
using System.Collections.Generic;

public class CollisionDetection : MonoBehaviour
{
    void Start()
    {
        if (HapticDeviceManager.Instance.GetAllDevices().Count != 0) {
            Debug.Log("Haptic Device Initialized");
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white, 10f);
        // }
        Debug.Log("Collision detected with " + collider.gameObject.name);

        HapticDeviceManager.Instance.SendHapticData(collider.gameObject.name);
    }

    private void OnTriggerExit(Collider collider)
    {
        //Debug.Log("Collision ended with " + collider.gameObject.name);

       HapticDeviceManager.Instance.SendHapticData(collider.gameObject.name);
    }

    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("Collision with " + collider.gameObject.name + " is still happening");

        HapticDeviceManager.Instance.SendHapticData(collider.gameObject.name);
    }
    private void OnApplicationQuit()
    {
        foreach(HapticDevice device in HapticDeviceManager.Instance.GetAllDevices()) {
            device.ClosePort();
        }
    }
}
