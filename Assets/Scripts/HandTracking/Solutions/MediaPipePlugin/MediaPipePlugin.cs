using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using Mediapipe.Unity.Dyspra;

public class MediaPipePlugin : MonoBehaviour, IHandTrackingSolution
{
    public string id => "mediapipe-plugin";
    public string displayName => "MediaPipe Plugin";

    public bool isTracking => _isTracking;
    private bool _isTracking = false;

    public GameObject settingsPrefab => _settingsPrefab;
    private GameObject _settingsPrefab = null;

    
    // private readonly object _lockObject = new object();
    private Thread initialisationThread;
    private bool _isInitialised = false;

    // public List<Vector3> handLandmarks 
    // {
    //     get 
    //     {
    //         // if (_solution == null || _solution.handLandmarks == null)
    //         // {
    //             return new List<Vector3>();
    //         // }
    //         // return _solution.handLandmarks.ConvertAll(v => new Vector3(v.Landmark.X, v.Landmark.Y, v.Landmark.Z));
    //     }
    // }

    public Vector3[] LeftHandLandmarks
    {
        get
        {
        //     lock (_lockObject)
        //     {
                return _solution == null ? new Vector3[21] : _solution.LeftHandLandmarks;
        //     }
        }
    }

    public Vector3 LeftHandPosition
    {
        get
        {
            // lock (_lockObject)
            // {
                return _solution == null ? new Vector3(0, 0, 0) : _solution.LeftHandPosition;
            // }
        }
    }

    public Vector3[] RightHandLandmarks
    {
        get
        {
            // lock (_lockObject)
            // {
                return _solution == null ? new Vector3[21] : _solution.RightHandLandmarks;
            // }
        }
    }

    public Vector3 RightHandPosition
    {
        get
        {
            // lock (_lockObject)
            // {
                return _solution == null ? new Vector3(0, 0, 0) : _solution.RightHandPosition;
            // }
        }
    }

    private MyBootstrap _bootstrap;
    private MyMediaPipeSolution _solution;
    private MyMediaPipeGraph _graph;
    private MyTextureFramePool _textureFramePool;
    private MyWebCamSource _webCamSource;

    public MyBootstrap bootstrap
    {
        get
        {
            // lock (_lockObject)
            // {
                return _bootstrap;
            // }
        }
    }

    public MyMediaPipeSolution solution
    {
        get
        {
            // lock (_lockObject)
            // {
                return _solution;
            // }
        }
    }


    public void Start()
    {
        
        _settingsPrefab = Resources.Load<GameObject>("HandTrackingSettingsPrefabs/MediaPipePluginSettings");
        string cpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_cpu").text;
        string gpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_gpu").text;
        string openGlEsConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_opengles").text;
        _textureFramePool = new MyTextureFramePool();
        // _webCamSource = gameObject.AddComponent<MyWebCamSource>();
        _bootstrap = gameObject.AddComponent<MyBootstrap>();

        UnityEngine.Debug.Log($"Current thread of awake MediaPipePlugin: {Thread.CurrentThread.ManagedThreadId}");

        initialisationThread = new Thread(() => InitializeDetection(cpuConfig, gpuConfig, openGlEsConfig));
        initialisationThread.Start();
    }

    private void InitializeDetection(string cpuConfig, string gpuConfig, string openGlEsConfig)
    {
        UnityEngine.Debug.Log($"Current thread of initialize detection MediaPipePlugin before lock: {Thread.CurrentThread.ManagedThreadId}");
        
        // lock (_lockObject)
        // {

            UnityEngine.Debug.Log("Attente de l'initialisation du bootstrap...");
            int tryCount = 0;
            while (_bootstrap.isFinished == false)
            {
                UnityEngine.Debug.Log("bootstrap finished: " + _bootstrap.isFinished);
                Thread.Sleep(100);
                // Monitor.Wait(_lockObject, 100);
                // if (tryCount > 100)
                // {
                    // UnityEngine.Debug.Log("Le bootstrap ne s'est pas initialisé correctement, on arrête l'initialisation du plugin MediaPipe");
                    // return;
                // }
                // tryCount++;
            }
            UnityEngine.Debug.Log("Initialisation du plugin MediaPipe...");

            UnityEngine.Debug.Log("Initialisation du plugin MediaPipe: création du graph");
            _graph = new MyMediaPipeGraph
            {
                _cpuConfig = cpuConfig,
                _gpuConfig = gpuConfig,
                _openGlEsConfig = openGlEsConfig
            };


            UnityEngine.Debug.Log("Initialisation du plugin MediaPipe: création de la solution");
            _solution = new MyMediaPipeSolution
            {
                runningMode = RunningMode.NonBlockingSync,
                bootstrap = _bootstrap,
                graphRunner = _graph,
                textureFramePool = _textureFramePool,
            };
            _isInitialised = true;
            UnityEngine.Debug.Log("Initialisation du plugin MediaPipe: fin de l'initialisation");
        // }
    }

    public async Task<bool> StartTracking()
    {
        UnityEngine.Debug.Log($"Current thread of play start tracking MediaPipePlugin: {Thread.CurrentThread.ManagedThreadId}");
        // lock (_lockObject)
        // {
            UnityEngine.Debug.Log("Démarrage du plugin MediaPipe...");
            int tryCount = 0;
            while (_isInitialised == false)
            {
                await Task.Delay(100);
                // Monitor.Wait(_lockObject, 100);
                UnityEngine.Debug.Log("isInitialised: " + _isInitialised);
                // if (tryCount > 500)
                // {
                //     UnityEngine.Debug.Log("Le plugin MediaPipe ne s'est pas initialisé correctement, on arrête le démarrage du tracking");
                //     return false;
                // }
                // tryCount++;
            }
            UnityEngine.Debug.Log("Démarrage du plugin MediaPipe après lock...");
            if (isTracking)
            {
                return false;
            }
            await Task.Delay(3000); // TODO: remove this delay, put to manually wait graph finish initialisation
            Task.Run(() =>
            {
                UnityEngine.Debug.Log("Play solution...");
                _solution.Play();
            });
            _isTracking = true;
            return true;
        // }
    }

    public async Task<bool>  StopTracking()
    {
        // lock (_lockObject)
        // {
            UnityEngine.Debug.Log("Arrêt du plugin MediaPipe...");
            while (_isInitialised == false)
            {
                // Monitor.Wait(_lockObject, 100);
            }
            if (!isTracking)
            {
                return false;
            }
            // while (_bootstrap.isFinished == false)
            // {
            //     Monitor.Wait(_lockObject, 100);
            // }
            Task.Run(() =>
            {
                _solution.Stop();
            }).Wait();
            _isTracking = false;
            return true;
        // }
    }

    public void SetScreen(Mediapipe.Unity.Dyspra.Screen screen)
    { 
        // lock (_lockObject)
        // {
            _solution.screen = screen;
            _solution.SetupScreen(MyImageSourceProvider.ImageSource);
        // }
    }
}
