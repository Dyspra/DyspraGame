using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class UDPClient
{
    private Socket _socket;
    private EndPoint _ep;

    private byte[] _buffer_recv;
    private ArraySegment<byte> _buffer_recv_segment;

    public void Initialize(IPAddress address, int port)
    {
        _buffer_recv = new byte[4096];
        _buffer_recv_segment = new ArraySegment<byte>(_buffer_recv);

        _ep = new IPEndPoint(address, port);

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Console.WriteLine("Socket créée !");
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
        Console.WriteLine("Options données !");
        /*_socket.Connect(_ep);
        Console.WriteLine("Socket connectée !");*/
    }

    public void StartMessageLoop()
    {
        _ = Task.Run(async () =>
        {
            SocketReceiveMessageFromResult res;
            while (true)
            {
                res = await _socket.ReceiveMessageFromAsync(_buffer_recv_segment, SocketFlags.None, _ep);
                Console.WriteLine(res.ReceivedBytes);
            }
        });
    }

    public async Task Recieve()
    {
        SocketReceiveFromResult res;
        res = await _socket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, _ep);
        Console.WriteLine(res.ReceivedBytes);
    }

    public async Task Send(byte[] data)
    {
        var s = new ArraySegment<byte>(data);
        await _socket.SendToAsync(s, SocketFlags.None, _ep);
    }
}
