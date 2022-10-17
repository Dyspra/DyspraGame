using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPServer: MonoBehaviour
{
    [SerializeField] private MovementManager movementManager;
    public const int PORT = 5000;
    public HandPosition HandsPosition;

    private Socket _socket;
    private EndPoint _ep;

    private byte[] _buffer_recv;
    private ArraySegment<byte> _buffer_recv_segment;

    public byte[] SubArray(byte[] data, int index, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }

    public void Initialize()
    {
        HandsPosition = new HandPosition();
        _buffer_recv = new byte[4096];
        _buffer_recv_segment = new ArraySegment<byte>(_buffer_recv);

        _ep = new IPEndPoint(IPAddress.Any, PORT);

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _socket.Bind(_ep);
    }

    public void StartMessageLoop(CancellationToken token)
    {
        _ = Task.Run(async () =>
        {
            SocketReceiveFromResult res;
            while (!token.IsCancellationRequested)
            {
                Debug.Log("TOKEN GOOD");
                await _socket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, _ep);
                var resArray = _buffer_recv_segment.Array;
                bool isAdded = false;
                for (int index = 0; index < 4096; index++) {
                    switch(resArray[index]) {
                        case var expression when (index >= 0 && index < 4):
                            uint portSource = BitConverter.ToUInt32(resArray, 0);
                            index += 4;
                            //Debug.Log("portSource = " + portSource);
                            break;
                        case var expression when (index >= 4 && index < 8):
                            uint destinationPort = BitConverter.ToUInt32(resArray, 4);
                            index += 4;
                            //Debug.Log("destinationPort = " + destinationPort);
                            break;
                        case var expression when (index >= 8 && index < 12):
                            uint length = BitConverter.ToUInt32(resArray, 8);
                            index += 4;
                            //Debug.Log("length = " + length);
                            break;
                        case var expression when (index >= 12 && index < 16):
                            uint checksum = BitConverter.ToUInt32(resArray, 12);
                            index += 4;
                            //Debug.Log("checksum = " + checksum);
                            break;
                        case var expression when (index >= 8):
                            string data = System.Text.Encoding.Default.GetString(SubArray(resArray, 16, 80));
                            int charLocation = data.IndexOf(',', 0);
                            double x = Convert.ToDouble(data.Substring(0, charLocation).Replace(".", ","));
                            int newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            double y = Convert.ToDouble(data.Substring(newBeginning, charLocation - newBeginning).Replace(".", ","));
                            newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            double z = Convert.ToDouble(data.Substring(newBeginning, charLocation - newBeginning).Replace(".", ","));
                            newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            int landmark = Int32.Parse(data.Substring(newBeginning, charLocation - newBeginning).Replace(".", ","));
                            newBeginning = charLocation + 1;
                            double date = Convert.ToDouble(data.Substring(newBeginning).Replace(".", ","));
                            index += 4096;
                            Package package = new Package(new Vector3((float)x, (float)y, (float)z), landmark);
                            if(date >= HandsPosition.date) {
                                if (HandsPosition.packages.Count == 0) {
                                    HandsPosition.packages.Add(package);
                                } else {
                                    foreach(var package_to_replace in HandsPosition.packages) {
                                        if (package_to_replace.landmark == landmark) {
                                            package_to_replace.position = package.position;
                                            package_to_replace.landmark = package.landmark;
                                            isAdded = true;
                                            break;
                                        }
                                    }
                                    if (isAdded == false) {
                                        HandsPosition.packages.Add(package);
                                    }
                                }
                                HandsPosition.date = date;
                                break;
                            }
                            break;
                        default:
                            Debug.Log("Error");
                            break;
                    }
                }
                movementManager.ApplyCoordinates(ref this.HandsPosition);
            }
            Debug.Log("STOP");
        }, token);
    }

    public async Task SendTo(EndPoint recipient, byte[] data)
    {
        var s = new ArraySegment<byte>(data);
        await _socket.SendToAsync(s, SocketFlags.None, recipient);
    }
}
