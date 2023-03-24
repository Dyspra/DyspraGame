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
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


public class MovementInterpretor : MonoBehaviour
{
   private bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
   private string _executablePath { get {
      UnityEngine.Debug.Log("get _executablePath");
      // clean path
      var binaryPath = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist/dyspra_hand_tracking_collector/dyspra_hand_tracking")).TrimEnd(Path.DirectorySeparatorChar);
      // todo: when building project, need to change the path to the game folder
      if (isWindows == true) {
         binaryPath = binaryPath.Replace("/", "\\");
         binaryPath += ".exe";
      }
      UnityEngine.Debug.Log("get _executablePath, binaryPath: " + binaryPath);
      if (File.Exists(binaryPath)) {
         UnityEngine.Debug.Log("get _executablePath, binaryPath exists");
         return binaryPath;
      }
      UnityEngine.Debug.Log("get _executablePath, binaryPath does not exists");
      return null;
   }}

   public CancellationTokenSource tokenSource;
   public UDPServer server;

   void OnPostprocessBuild(BuildTarget target, string path)
   {
      UnityEngine.Debug.Log("OnPreprocessBuild for target " + target + " at path " + path);
      // todo: build python script in the build folder
   }

   void Awake()
   {
      StartUDPServer();
      UnityEngine.Debug.Log("isWindows: " + isWindows);
      UnityEngine.Debug.Log("_executablePath: " + _executablePath);
      UnityEngine.Debug.Log("Application.isEditor: " + Application.isEditor);
      if (_executablePath == null && Application.isEditor)
      {
         UnityEngine.Debug.Log("Awake: BuildPythonScript");
         BuildPythonScript().ContinueWith((task) => {
            UnityEngine.Debug.Log("Awake: BuildPythonScript continuewith task.Result: " + task.Result);
            if (task.Result)
            {
               UnityEngine.Debug.Log("Awake: LaunchPythonScript after build success");
               this.LaunchPythonScript();
            }
         });
         UnityEngine.Debug.Log("Awake: BuildPythonScript Done");
      } else {
         UnityEngine.Debug.Log("Awake: LaunchPythonScript");
         this.LaunchPythonScript();
      }
   }
   
   void StartUDPServer()
   {
      tokenSource = new CancellationTokenSource();
      server.Initialize();
      UnityEngine.Debug.Log("Démarrage du serveur UDP...");
      server.StartMessageLoop(tokenSource.Token);
   }

   Task<bool> BuildPythonScript()
   { 
      var tcs = new TaskCompletionSource<bool>();
      try {
         string specPath = Path.Combine(Application.dataPath, "Plugins/MediapipePythonInterface/dyspra_hand_tracking.spec");

         if (isWindows == true)
         {
            specPath = specPath.Replace("/", "\\");
         }

         UnityEngine.Debug.Log("specPath: " + specPath);
         if (!File.Exists(specPath))
         {
            UnityEngine.Debug.Log("Python script does not exists in the current context");
            return Task.FromResult(false);
         }
         UnityEngine.Debug.Log("Building Python script in " + Application.persistentDataPath);

         var buildProcess = new Process
         {
            StartInfo = {
               FileName = "pyinstaller",
               Arguments = "--workpath " + Application.persistentDataPath + "/MediapipePythonInterface/build --distpath " + Application.persistentDataPath + "/MediapipePythonInterface/dist --clean --noconfirm " + specPath,
               // todo: when building project, need to change the path to the game folder
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
         // process.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface] " + args.Data); // todo: make it toggable, because it spam the console
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
}
