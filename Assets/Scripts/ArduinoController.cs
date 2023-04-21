using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    private ArduinoHapticDevice arduino_port;
    public bool vibration_start = false;
    public bool close_port = false;
    public string com_port = "COM9";
    public int port_nb = 9600;

    void Start()
    {
        arduino_port = new ArduinoHapticDevice(com_port, port_nb);
    }
    void Update()
    {
        if (vibration_start) {
            VibrationStart();
        } else if (close_port) {
            ClosePort();
        }
    }
    void VibrationStart()
    {
        arduino_port.SendData("1");
    }
    void ClosePort()
    {
        arduino_port.ClosePort();
    }
}