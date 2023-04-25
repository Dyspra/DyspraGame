using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoHapticDevice : HapticDevice
{
    SerialPort arduino_port;
    bool retry = false;
    //bool IsConnected = false;
    public ArduinoHapticDevice(string com_port, int port_nb)
    {
        this.id = "arduin-haptic-device-1";
        while (retry == false) {
            Debug.Log("Trying to connect...");
            retry  = OpenPort(com_port, port_nb);

        };
        Debug.Log("Arduino device created");
    }

    // Will be useful when we will have more than one motor
    public static new List<HapticDevice> GetAvailableDevices()
    {
        List<HapticDevice> devices = new List<HapticDevice>();
        string[] ports = SerialPort.GetPortNames();
        /* foreach (string port in ports)
        {
            ArduinoHapticDevice device = new ArduinoHapticDevice(port, DeviceConnectionType.COM);
            if (device.IsConnected)
            {
                AddDevice(device);
            }
        } */
        devices.Add(new ArduinoHapticDevice("COM3", 9600));
        return devices;
    }
    public override bool OpenPort(string com_port, int port_nb)
    {
        arduino_port = new SerialPort(com_port, port_nb);
        try {
            arduino_port.Open();
        } catch {
            Debug.Log("Port wasn't initialized, the port may be busy.");
            return false;
        }
        return true;
    }

    public override void SendData(string command)
    {
        if (arduino_port.IsOpen == true) {
            Debug.Log("Send data to Arduino device " + this.id + " : " + command);
            arduino_port.Write(command);
        }
    }
    public override void ClosePort()
    {
        if (arduino_port.IsOpen == true) {
            Debug.Log("Closing Arduino device connection");
            arduino_port.Close();
        }
    }
}