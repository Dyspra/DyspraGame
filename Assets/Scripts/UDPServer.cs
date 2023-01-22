using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;

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
                //Debug.Log("TOKEN GOOD");
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
                            //Debug.Log("data = " + data);
                            int charLocation = data.IndexOf(',', 0);
                            float x = float.Parse(data.Substring(0, charLocation).Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                            int newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            float y = float.Parse(data.Substring(newBeginning, charLocation - newBeginning).Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                            newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            float z = float.Parse(data.Substring(newBeginning, charLocation - newBeginning).Replace(",", "."), CultureInfo.InvariantCulture.NumberFormat);
                            newBeginning = charLocation + 1;
                            charLocation = data.IndexOf(',', charLocation + 1);
                            int landmark = Int32.Parse(data.Substring(newBeginning, charLocation - newBeginning).Replace(".", ","));
                            Debug.Log("Landmark = " + landmark);
                            newBeginning = charLocation + 1;
                            double date = Convert.ToDouble(data.Substring(newBeginning).Replace(".", ","));
                            index += 4096;
                            //Debug.Log("float = " + y );
                            Package package = new Package(new Vector3(x, y, z), landmark);
                            if (HandsPosition.packages.Count == 0) {
                                HandsPosition.packages.Add(package);
                            } else {
                                foreach(var package_to_replace in HandsPosition.packages) {
                                    if (package_to_replace.landmark == landmark) {
                                        package_to_replace.position = package.position;
                                        isAdded = true;
                                        //Debug.Log("J'update l'élément avec la landmark n°" + package_to_replace.landmark);
                                        break;
                                    }
                                }

                                if (isAdded == false) {
                                    //Debug.Log("J'ajoute un nouvel élément");
                                    HandsPosition.packages.Add(package);
                                }
                            }
                            break;
                        default:
                            //Debug.Log("Error");
                            break;
                    }
                }
            }
            //Debug.Log("STOP");
        }, token);
    }

    public async Task SendTo(EndPoint recipient, byte[] data)
    {
        var s = new ArraySegment<byte>(data);
        await _socket.SendToAsync(s, SocketFlags.None, recipient);
    }
}
