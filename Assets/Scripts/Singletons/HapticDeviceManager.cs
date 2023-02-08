using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using Dyspra;

public class HapticDeviceManager : Singleton<HapticDeviceManager>
{
    private List<HapticDevice> _connectedDevices;


    private void Awake() {
        // check any connected device on port com
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            HapticDevice device = new HapticDevice(port, DeviceConnectionType.COM);
            if (device.IsConnected)
            {
                AddDevice(device);
            }
        }

        // check any connected device on bluetooth
        // TODO
    }

    private void Start()
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
