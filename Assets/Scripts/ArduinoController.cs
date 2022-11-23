using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort arduino_port;
    public bool led_open = false;

    void Start()
    {
        arduino_port = new SerialPort("COM5", 9600);
        try {
            arduino_port.Open();
        } catch {
            Debug.Log("Port wasn't initialized, the port may be busy.");
        }
    }
    void Update()
    {
        if (led_open) {
            open_led();
        }
    }
    void open_led()
    {
        if (arduino_port.IsOpen) {
            arduino_port.Write("1");
        }
    }
}