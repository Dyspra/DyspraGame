public abstract class HapticDevice
{

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void SendCommand(string command);
}
