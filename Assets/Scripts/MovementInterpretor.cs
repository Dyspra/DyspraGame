using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInterpretor : MonoBehaviour
{
    public CancellationTokenSource tokenSource;
    // Start is called before the first frame update
    void Start()
    {
        tokenSource = new CancellationTokenSource();
        var server = new UDPServer();
        server.Initialize();
        server.StartMessageLoop(tokenSource.Token);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        tokenSource.Cancel();
    }
}
