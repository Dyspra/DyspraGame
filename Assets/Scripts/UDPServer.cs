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
    public const int PORT = 5000;

    private Socket _socket;
    private EndPoint _ep;

    private byte[] _buffer_recv;
    private ArraySegment<byte> _buffer_recv_segment;

    public void Initialize()
    {
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
                res = await _socket.ReceiveFromAsync(_buffer_recv_segment, SocketFlags.None, _ep);
                Debug.Log(res.ReceivedBytes);
                await SendTo(res.RemoteEndPoint, Encoding.UTF8.GetBytes("Hello back!"));
            }
        }, token);
    }

    public async Task SendTo(EndPoint recipient, byte[] data)
    {
        var s = new ArraySegment<byte>(data);
        await _socket.SendToAsync(s, SocketFlags.None, recipient);
    }
}
