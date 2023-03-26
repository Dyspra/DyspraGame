using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort arduino_port;
    public bool vibration_start = false;
    public bool close_port = false;
    public string com_port = "COM9";

    void Start()
    {
        arduino_port = new SerialPort(com_port, 9600);
        try {
            arduino_port.Open();
        } catch {
            Debug.Log("Port wasn't initialized, the port may be busy.");
        }
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
        if (arduino_port.IsOpen == true) {
            try {
                Debug.Log("1");
                arduino_port.Write("1");
            } catch {
                Debug.Log("Write Timeout");
            }
        } else {
            Debug.Log("Port isn't open !!!");
        }
    }
    void ClosePort()
    {
        if (arduino_port.IsOpen == true) {
            arduino_port.Close();
        }
    }
}