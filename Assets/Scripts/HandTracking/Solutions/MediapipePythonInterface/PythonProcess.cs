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
#if UNITY_EDITOR
   using UnityEditor;
   using UnityEditor.Build;
   using UnityEditor.Build.Reporting;
#endif

public class PythonProcess
#if UNITY_EDITOR
   : IPostprocessBuildWithReport
#endif
{
   private static bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
   
   private Process _process;
   #if UNITY_EDITOR

      private Process _buildProcess;
      private string _binaryPath;

      public void StartProcess()
      {
         _binaryPath = ConvertToPlatformPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/dist/dyspra_hand_tracking/dyspra_hand_tracking"));
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

      public int callbackOrder { get { return 0; } }

public void OnPostprocessBuild(BuildReport report)
{
      UnityEngine.Debug.Log("OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
      UnityEngine.Debug.Log("Data folder of output path: " + Path.Combine(Path.GetDirectoryName(report.summary.outputPath), Path.GetFileNameWithoutExtension(report.summary.outputPath) + "_Data"));

      // Display the dialog and get the result
      bool result = EditorUtility.DisplayDialog("Build Mediapipe python interface", "Do you want to build the Mediapipe Python Interface?", "Yes", "No");

      // Check if the result is "No"
      if (!result)
      {
         // Cancel the BuildPythonScript
         return;
      }
      // Display the progress bar
      bool cancelled = false;
      EditorUtility.DisplayCancelableProgressBar("Build Mediapipe python interface", "Building Mediapipe Python Interface...", 0.0f);

      var ts = new CancellationTokenSource();
      CancellationToken ct = ts.Token;
      Task<bool> buildPythonScriptTask = this.BuildPythonScript(
         ConvertToPlatformPath(Path.Combine(Application.dataPath, "Plugins/MediapipePythonInterface/dyspra_hand_tracking.spec")),
         ConvertToPlatformPath(Path.Combine(Application.persistentDataPath, "MediapipePythonInterface/build")),
         ConvertToPlatformPath(Path.Combine(Path.GetDirectoryName(report.summary.outputPath), "Plugins/MediapipePythonInterface")),
         ct
      );

      // Update the progress bar while the build is running
      while (!buildPythonScriptTask.IsCompleted && !cancelled)
      {
         cancelled = EditorUtility.DisplayCancelableProgressBar("Build Mediapipe python interface", "Building Mediapipe Python Interface...", 1.0f);
      }

      // Hide the progress bar
      EditorUtility.ClearProgressBar();

      if (cancelled)
      {
         ts.Cancel();
         UnityEngine.Debug.LogError("OnPreprocessBuild: BuildPythonScript cancelled");
         return;
      }

      // Check if the build was successful
      if (buildPythonScriptTask.Result)
      {
         UnityEngine.Debug.Log("OnPreprocessBuild: BuildPythonScript finished successfully");
         EditorUtility.DisplayDialog("Build Python Script", "BuildPythonScript finished successfully", "OK");
      }
      else
      {
         UnityEngine.Debug.LogError("OnPreprocessBuild: BuildPythonScript failed");
         EditorUtility.DisplayDialog("Build Python Script", "BuildPythonScript failed", "OK");
      }
}

      Task<bool> BuildPythonScript(string specPath, string tempPath, string distPath, CancellationToken ct = default(CancellationToken))
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
            if (!Directory.Exists(distPath))
            {
               Directory.CreateDirectory(distPath);
            }

            if (this._buildProcess != null)
            {
               this._buildProcess.Kill();
               this._buildProcess.Dispose();
            }
            this._buildProcess = new Process
            {
               StartInfo = {
                  FileName = "pyinstaller",
                  Arguments = "--workpath " + tempPath + " --distpath " + distPath + " --clean --noconfirm " + specPath,
                  UseShellExecute = false,
                  RedirectStandardOutput = true,
                  RedirectStandardError = true,
                  CreateNoWindow = true
               },
               EnableRaisingEvents = true,
            };
            this._buildProcess.Exited += (sender, args) => {
               UnityEngine.Debug.Log("buildProcess.Exited " + this._buildProcess.ExitCode);
               tcs.SetResult(this._buildProcess.ExitCode == 0);
            };
            this._buildProcess.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface installer] " + args.Data);
            // todo: normal log goes to stderr instead of stdout
            this._buildProcess.ErrorDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface installer stderr] " + args.Data);
            UnityEngine.Debug.Log("_buildProcess start");
            bool started = this._buildProcess.Start();
            UnityEngine.Debug.Log("_buildProcess.Started: " + started);
            if (!started)
            {
               UnityEngine.Debug.Log("buildProcess.Started: " + started);
               tcs.SetResult(false);
            }
            this._buildProcess.BeginOutputReadLine();
            this._buildProcess.BeginErrorReadLine();
            if (ct != default(CancellationToken))
            {
               ct.Register(() => {
                  UnityEngine.Debug.Log("_buildProcess ct.Register");
                  this._buildProcess.Kill();
                  tcs.SetResult(false);
               });
            }
         } catch (Exception e) {
            UnityEngine.Debug.LogError("MediapipePythonInterface install Exception: " + e);
            tcs.SetResult(false);
            return tcs.Task;
         }
            return tcs.Task;
      }
   #elif UNITY_STANDALONE
      private string _binaryPath;

      public void StartProcess()
      {
         // system print
         UnityEngine.Debug.LogError("Force the dev console open...");
         _binaryPath = ConvertToPlatformPath(Path.Combine(Application.dataPath, "../Plugins/MediapipePythonInterface/dyspra_hand_tracking/dyspra_hand_tracking"), true);
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
         if (this._process != null)
         {
            this._process.Kill();
            this._process.Dispose();
         }
         this._process = new Process
         {
            StartInfo = {
               WorkingDirectory = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(_executablePath)).TrimEnd(Path.DirectorySeparatorChar)), // todo: make it platform agnostic
               FileName = _executablePath,
               Arguments = "5000 127.0.0.1", // todo: when 5000 is not available, change it to another port
               UseShellExecute = false,
               RedirectStandardOutput = true,
               RedirectStandardError = true,
               CreateNoWindow = true,
            },
            EnableRaisingEvents = true,
         };
         this._process.Exited += (sender, args) => {
            UnityEngine.Debug.Log("process.Exited " + this._process.ExitCode);
            tcs.SetResult(this._process.ExitCode == 0);
         };
         this._process.OutputDataReceived += (sender, args) => UnityEngine.Debug.Log("[MediapipePythonInterface] " + args.Data); // todo: make it toggable, because it spam the console
         this._process.ErrorDataReceived += (sender, args) => UnityEngine.Debug.LogError("[MediapipePythonInterface] " + args.Data);
         UnityEngine.Debug.Log("process start");
         bool started = this._process.Start(); // todo: stop the process when the game is closed
         UnityEngine.Debug.Log("process.Started: " + started);
         if (!started)
         {
            UnityEngine.Debug.Log("process.Started: " + started);
            tcs.SetResult(false);
         }
         this._process.BeginOutputReadLine();
         this._process.BeginErrorReadLine();
      } catch (Exception e) {
         UnityEngine.Debug.LogError("MediapipePythonInterface Exception: " + e);
         tcs.SetResult(false);
      }
      return tcs.Task;
   }        

   ~PythonProcess()
   {
      this.StopProcess();
   }

   public void StopProcess()
   {
      if (this._process != null && !this._process.HasExited)
      {
         this._process.Kill();
         this._process.Dispose();
      }

#if UNITY_EDITOR
      if (this._buildProcess != null && !this._buildProcess.HasExited)
      {
         this._buildProcess.Kill();
         this._buildProcess.Dispose();
      }
#endif

      if (this.tokenSource != null)
      {
         this.tokenSource.Cancel();
      }
   }
   private static string ConvertToPlatformPath(string path, bool isExecutablePath = false)
   {
      if (isWindows == true)
      {
         path = path.Replace("/", "\\");
      }
      if (isExecutablePath == true)
      {
         path += ".exe";
      }
      return path;
   }
}

