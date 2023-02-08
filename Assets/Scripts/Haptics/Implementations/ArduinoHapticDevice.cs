using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoHapticDevice : HapticDevice
{
    ArduinoHapticDevice()
    {
        this.id = "arduin-haptic-device-1";
        Debug.Log("Arduino device created");
    }

    public static new List<HapticDevice> GetAvailableDevices()
    {
        Debug.Log("Get available devices for ArduinoHapticDevice");
        List<HapticDevice> devices = new List<HapticDevice>();

        // todo:
        // // check any connected device on port com
        // string[] ports = SerialPort.GetPortNames();
        // foreach (string port in ports)
        // {
        //     HapticDevice device = new HapticDevice(port, DeviceConnectionType.COM);
        //     if (device.IsConnected)
        //     {
        //         AddDevice(device);
        //     }
        // }
        devices.Add(new ArduinoHapticDevice());
        return devices;
    }

    public override void SendData(string command)
    {
        Debug.Log("Send data to Arduino device " + this.id + " : " + command);
        // todo
    }
}