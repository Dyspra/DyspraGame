using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class ArduinoHandFeedBack : MonoBehaviour
{
    SerialPort arduino_port;
    public FingerFeedback finger1;
    public FingerFeedback finger2;
    public FingerFeedback finger3;
    public FingerFeedback finger4;
    public FingerFeedback finger5;

    void Start()
    {
        arduino_port = new SerialPort("COM5", 9600);
        finger1.finger_nb = '1';
        finger2.finger_nb = '2';
        finger3.finger_nb = '3';
        finger4.finger_nb = '4';
        finger5.finger_nb = '5';
        try {
            arduino_port.Open();
            finger1.arduino_port = arduino_port;
            finger2.arduino_port = arduino_port;
            finger3.arduino_port = arduino_port;
            finger4.arduino_port = arduino_port;
            finger5.arduino_port = arduino_port;
        } catch {
            Debug.Log("Port wasn't initialized, the port may be busy.");
        }
    }

    void send_vibration(char finger, Vector2 pos)
    {
        switch(finger)
        {
            case '1':
                finger1.finger.x = pos.x;
                finger1.finger.y = pos.y;
                break;
            case '2':
                finger2.finger.x = pos.x;
                finger2.finger.y = pos.y;
                break;
            case '3':
                finger3.finger.x = pos.x;
                finger3.finger.y = pos.y;
                break;
            case '4':
                finger4.finger.x = pos.x;
                finger4.finger.y = pos.y;
                break;
            case '5':
                finger5.finger.x = pos.x;
                finger5.finger.y = pos.y;
                break;
            default:
                Debug.Log("Finger not found");
                break;
        }
    }

}