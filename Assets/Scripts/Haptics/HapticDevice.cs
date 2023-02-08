public enum DeviceConnectionType
{
    Bluetooth,
    COM
}

public abstract class HapticDevice
{
    public DeviceConnectionType ConnectionType { get; protected set; }
    public bool IsConnected { get; protected set; }

    public HapticDevice(string port, DeviceConnectionType ConnectionType)
    {
        Debug.Log("HapticDevice created");
        this.ConnectionType = ConnectionType;
        this.IsConnected = Connect();
    }
    public abstract bool Connect();
    public abstract bool Disconnect();
    public abstract bool SendCommand(string command);
}
