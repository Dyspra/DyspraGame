using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoController : MonoBehaviour
{
    SerialPort arduino_port;
    public bool led_open = false;
    public bool led_flash = false;

    void Start()
    {
        arduino_port = new SerialPort("COM6", 9600);
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
        } else if (led_flash) {
            flash_led();
        } else {
            close_led();
        }
    }
    void open_led()
    {
        if (arduino_port.IsOpen) {
            arduino_port.Write("1");
        }
    }
    void close_led()
    {
        if (arduino_port.IsOpen) {
            arduino_port.Write("2");
        }
    }
    void flash_led()
    {
        if (arduino_port.IsOpen) {
            arduino_port.Write("3");
            arduino_port.Write("2");
        }
    }
}