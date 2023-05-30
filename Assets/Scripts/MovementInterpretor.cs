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
using UnityEditor;


public class MovementInterpretor : MonoBehaviour
{
   private bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

   #if UNITY_EDITOR
      // private string specPath = Path.Combine(Application.dataPath, "Plugins/MediapipePythonInterface/dyspra_hand_tracking.spec");
      // private string tempPath = Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/build");
      // private string distPath = Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist");
      private string _binaryPath = this.ConvertToPlatformPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist/dyspra_hand_tracking/dyspra_hand_tracking"));

      void Awake()
      {
         StartUDPServer();
         UnityEngine.Debug.Log("isWindows: " + isWindows);
         UnityEngine.Debug.Log("_executablePath: " + _executablePath);
         UnityEngine.Debug.Log("Application.isEditor: " + Application.isEditor);
         if (_executablePath == null)
         {
            UnityEngine.Debug.Log("Awake: BuildPythonScript");
            BuildPythonScript(
               ConvertToPlatformPath(Path.Combine(Application.dataPath, "Plugins/MediapipePythonInterface/dyspra_hand_tracking.spec")),
               ConvertToPlatformPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/build")),
               ConvertToPlatformPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist"))
            ).ContinueWith((task) => {
               UnityEngine.Debug.Log("Awake: BuildPythonScript continuewith task.Result: " + task.Result);
               if (task.Result)
               {
                  UnityEngine.Debug.Log("Awake: LaunchPythonScript after build success");
                  Thread.Sleep(2000);
                  this.LaunchPythonScript();
               }
            });
            UnityEngine.Debug.Log("Awake: BuildPythonScript Done");
         } else {
            UnityEngine.Debug.Log("Awake: LaunchPythonScript");
            this.LaunchPythonScript();
         }
      }

      void OnPostprocessBuild(BuildReport report)
      {
         Debug.Log("OnPostprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);

         // Build the python script
         bool success = this.BuildPythonScript();
      }

      Task<bool> BuildPythonScript(string specPath, string tempPath, string distPath)
      { 
         var tcs = new TaskCompletionSource<bool>();
         try {
            UnityEngine.Debug.Log("specPath: " + specPath);
            if (!File.Exists(specPath))
            {
               UnityEngine.Debug.Log("Python script does not exists in the current context");
               return Task.FromResult(false);
            }
         UnityEngine.Debug.Log("Building Python script in " + distPath);

            var buildProcess = new Process
            {
               StartInfo = {
                  FileName = "pyinstaller",
                  Arguments = "--workpath " + tempPath + "--distpath " + distPath + " --clean --noconfirm " + specPath,
                  UseShellExecute = false,
                  RedirectStandardOutput = true,
                  RedirectStandardError = true,
                  CreateNoWindow = true
               },
               EnableRaisingEvents = true,
            };
            buildProcess.Exited += (sender, args) => {
               UnityEngine.Debug.Log("buildProcess.Exited " + buildProcess.ExitCode);
               tcs.SetResult(buildProcess.ExitCode == 0);
            };
            buildProcess.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface installer] " + args.Data);
            // todo: normal log goes to stderr instead of stdout
            buildProcess.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("[MediapipePythonInterface installer] " + args.Data);
            UnityEngine.Debug.Log("buildProcess start");
            bool started = buildProcess.Start();
            UnityEngine.Debug.Log("buildProcess.Started: " + started);
            if (!started)
            {
               UnityEngine.Debug.Log("buildProcess.Started: " + started);
               tcs.SetResult(false);
            }
            buildProcess.BeginOutputReadLine();
            buildProcess.BeginErrorReadLine();
         } catch (Exception e) {
            UnityEngine.Debug.LogError("MediapipePythonInterface install Exception: " + e);
            tcs.SetResult(false);
            return tcs.Task;
         }
            return tcs.Task;
      }
   #elif UNITY_STANDALONE
      private string _binaryPath = this.ConvertToPlatformPath(Path.Combine(Application.dataPath, "Plugins/MediapipePythonInterface/dyspra_hand_tracking"));

      void Awake()
      {
         this.StartUDPServer();
         this.LaunchPythonScript();
      }
   #endif

   private string _executablePath { get {
      UnityEngine.Debug.Log("get _executablePath, binaryPath: " + this._binaryPath);
      if (File.Exists(this._binaryPath)) {
         UnityEngine.Debug.Log("get _executablePath, binaryPath exists");
         return this._binaryPath;
      }
      UnityEngine.Debug.Log("get _executablePath, binaryPath does not exists");
      return null;
   }}

   public CancellationTokenSource tokenSource;
   public UDPServer server;

   void StartUDPServer()
   {
      tokenSource = new CancellationTokenSource();
      server.Initialize();
      UnityEngine.Debug.Log("Démarrage du serveur UDP...");
      server.StartMessageLoop(tokenSource.Token);
   }

   Task<bool> LaunchPythonScript()
   {
      UnityEngine.Debug.Log("LaunchPythonScript");
      UnityEngine.Debug.Log("LaunchPythonScript, executablePath: " + _executablePath);
      var tcs = new TaskCompletionSource<bool>();
      if (_executablePath == null) {
         UnityEngine.Debug.Log("Python script does not exists in the current context");
         return Task.FromResult(false);
      }

      try {
         var process = new Process
         {
            StartInfo = {
               WorkingDirectory = Path.Combine(Path.GetFullPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist/dyspra_hand_tracking")).TrimEnd(Path.DirectorySeparatorChar)),
               FileName = _executablePath,
               Arguments = "5000 127.0.0.1", // todo: when 5000 is not available, change it to another port
               UseShellExecute = false,
               RedirectStandardOutput = true,
               RedirectStandardError = true,
               CreateNoWindow = true,
            },
            EnableRaisingEvents = true,
         };
         process.Exited += (sender, args) => {
            UnityEngine.Debug.Log("process.Exited " + process.ExitCode);
            tcs.SetResult(process.ExitCode == 0);
         };
         process.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface] " + args.Data); // todo: make it toggable, because it spam the console
         process.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("[MediapipePythonInterface] " + args.Data);
         UnityEngine.Debug.Log("process start");
         bool started = process.Start(); // todo: stop the process when the game is closed
         UnityEngine.Debug.Log("process.Started: " + started);
         if (!started)
         {
            UnityEngine.Debug.Log("process.Started: " + started);
            tcs.SetResult(false);
         }
         process.BeginOutputReadLine();
         process.BeginErrorReadLine();
      } catch (Exception e) {
         UnityEngine.Debug.LogError("MediapipePythonInterface Exception: " + e);
         tcs.SetResult(false);
      }
      return tcs.Task;
   }        

   void OnDestroy()
   {
      tokenSource.Cancel();
   }
   private string ConvertToPlatformPath(string path, bool isExecutablePath = false)
   {
      if (isWindows == true)
      {
         path = path.Replace("/", "\\");
      }
      if (isExecutable == true)
      {
         path += ".exe";
      }
      return path;
   }
}

