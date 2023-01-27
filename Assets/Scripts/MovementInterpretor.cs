using System;
using System.Net;
using System.Runtime;
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
   public UDPServer server;
    // Start is called before the first frame update
   void Start()
   {
      var path = "";
      tokenSource = new CancellationTokenSource();
      server.Initialize();
      UnityEngine.Debug.Log("Démarrage du serveur...");
      server.StartMessageLoop(tokenSource.Token);
      var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
      path = Path.Combine(Application.dataPath, "python_interface/dyspra_hand_tracking.py");
      if (isWindows == true) {
         path = path.Replace("/", "\\");
      }

      if (File.Exists(path)) {
         Process.Start(
            "py" + (isWindows ? ".exe" : ""),
            path + " 5000 127.0.0.1"
         );
         UnityEngine.Debug.Log("Serveur opérationnel !");
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
      tokenSource.Cancel();
   }
}
