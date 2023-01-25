using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class HapticDeviceManager : MonoBehaviour
{
    private List<HapticDevice> _connectedDevices;

    void Start()
    {
        _connectedDevices = new List<HapticDevice>();
    }

    public void AddDevice(HapticDevice device)
    {
        _connectedDevices.Add(device);
    }

    public void RemoveDevice(HapticDevice device)
    {
        _connectedDevices.Remove(device);
    }

    public void SendHapticCommand(string command)
    {
        foreach (HapticDevice device in _connectedDevices)
        {
            device.SendCommand(command);
        }
    }
}
