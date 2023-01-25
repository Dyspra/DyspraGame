public abstract class HapticDevice
{
    protected SerialPort _serialPort;

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void SendCommand(string command);
}
