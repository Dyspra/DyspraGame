using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class FingerFeedback : MonoBehaviour
{
    public Vector2 finger;
    public char finger_nb;
    public SerialPort arduino_port;


    void Start()
    {
        finger = Vector2.zero;
    }

    void Update()
    {
        if (finger.x != 0 || finger.y != 0) {
            finger_send();
            finger.x = 0;
            finger.y = 0;
        }
    }
    void finger_send()
    {
        if (arduino_port.IsOpen) {
            arduino_port.Write(finger_nb.ToString());
            // arduino_port.Write(finger.x.ToString());
            // arduino_port.Write(finger.y.ToString());
        } 
    }
}
