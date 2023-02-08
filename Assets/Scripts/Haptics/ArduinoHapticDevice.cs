using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoHapticDevice : HapticDevice
{
    private string _deviceName;
    private DeviceConnectionType _connectionType;
    private SerialPort _serialPort;

    public ArduinoHapticDevice(string deviceName, DeviceConnectionType connectionType)
    {
        Debug.Log("ArduinoHapticDevice created");
        _deviceName = deviceName;
        _connectionType = connectionType;
    }

    public override bool Connect()
    {
        if (_connectionType == DeviceConnectionType.COM)
        {
            _serialPort = new SerialPort(_deviceName, 9600);
            _serialPort.Open();
        }
        else if (_connectionType == DeviceConnectionType.Bluetooth)
        {
            // Code for connecting to Bluetooth device goes here
        }
    }

    public override bool Disconnect()
    {
        if (_connectionType == DeviceConnectionType.COM)
        {
            _serialPort.Close();
        }
        else if (_connectionType == DeviceConnectionType.Bluetooth)
        {
            // Code for disconnecting from Bluetooth device goes here
        }
    }

    public override bool SendCommand(string command)
    {
        if (_connectionType == DeviceConnectionType.COM)
        {
            _serialPort.Write(command.ToString());
        }
        else if (_connectionType == DeviceConnectionType.Bluetooth)
        {
            // Code for sending command over Bluetooth goes here
        }
    }
}