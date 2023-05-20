using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using Dyspra;

/// <summary>
///     HapticDeviceManager is a singleton that manages all available haptic devices.
///     It is used to select a device and send data to it.
/// </summary>
public class HapticDeviceManager : SingletonGameObject<HapticDeviceManager>
{
    private List<HapticDevice> _connectedDevices = new List<HapticDevice>();
    public HapticDevice device;

    private void Awake()
    {
        // get all available devices
        this.GetImplementationsAvailableDevices();
        // select first device
        if (this._connectedDevices.Count > 0)
        {
            this.device = this._connectedDevices[0];
        }
    }

    private void Update()
    {
        // each 3 seconds, check if there are new devices
        if (Time.time % 3 == 0)
        {
            this.GetImplementationsAvailableDevices();
        }
    }

    protected bool RemoveHapticDevice(HapticDevice device)
    {
        if (this.device == device)
        {
            this.device = null;
        }
        bool deviceRemoved = this._connectedDevices.Remove(device);

        // select a new device if the current one was removed
        if (this.device == null && this._connectedDevices.Count > 0)
        {
            this.device = this._connectedDevices[0];
        }
        return deviceRemoved;
    }

    private void GetImplementationsAvailableDevices()
    {
        // get all available devices from all implementations

        // ArduinoHapticDevice devices
        List<HapticDevice> arduinoDevices = ArduinoHapticDevice.GetAvailableDevices();

        // verify if it's new devices
        foreach (HapticDevice device in arduinoDevices)
        {
            if (!this._connectedDevices.Contains(device))
            {
                this._connectedDevices.Add(device);
            }
        }
    }

    public bool ChangeSelectedDevice(string id)
    {
        foreach (HapticDevice device in _connectedDevices)
        {
            if (device.id == id)
            {
                this.device = device;
                return true;
            }
        }
        return false;
    }

    public List<HapticDevice> GetAllDevices()
    {
        return this._connectedDevices;
    }

    public void SendHapticData(string command)
    {
        if (this.device == null)
        {
            // Debug.LogWarning("No device selected");
            return;
        }
        this.device.SendData(command);
    }
}
