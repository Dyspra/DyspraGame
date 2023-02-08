using System.Collections.Generic;
using UnityEngine;
public enum DeviceConnectionType
{
    Bluetooth,
    COM
}

interface IHapticDevice
{
    string id { get; }
    void SendData(string command);
}

public abstract class HapticDevice : IHapticDevice
{
    public string id { get; protected set; }

    public abstract void SendData(string command);

    /// <summary>
    /// > This function returns a list of all the haptic devices that are currently connected to the
    /// computer
    /// </summary>
    public static List<HapticDevice> GetAvailableDevices()
    {
        throw new System.NotImplementedException();
    }
}