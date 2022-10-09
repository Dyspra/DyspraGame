using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MovementInterpretor : MonoBehaviour
{
    public CancellationTokenSource tokenSource;
    // Start is called before the first frame update
    void Start()
    {
        // var files = Directory.EnumerateFiles("C:\\", "*dyspra_hand_tracking.py*", SearchOption.AllDirectories);
        // string[] dirs = Directory.GetFiles(@"c:\", "*" + filetofind);
        // tokenSource = new CancellationTokenSource();
        // var server = new UDPServer();
        // server.Initialize();
        // server.StartMessageLoop(tokenSource.Token);
        var path = Path.Combine(Application.persistentDataPath, "\\python_interface\\dyspra_hand_tracking.py");
        UnityEngine.Debug.Log(Application.dataPath + "/python_interface/dyspra_hand_tracking.py");
        if (File.Exists(Application.dataPath + "/python_interface/dyspra_hand_tracking.py")) {
            Process.Start("python3.exe", "C:\\Users\\$2P2100-B0HQ5KT4OKR2\\Documents\\GitHub\\DyspraGame\\python_interface\\dyspra_hand_tracking.py 6542 127.0.0.1");
        } else {
            UnityEngine.Debug.Log("Python script does not exists in the current context");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        // tokenSource.Cancel();
    }
}
