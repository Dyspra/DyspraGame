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

    
    private readonly object _lockObject = new object();
    private Thread initialisationThread;

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
            lock (_lockObject)
            {
                return _solution == null ? new Vector3[21] : _solution.LeftHandLandmarks;
            }
        }
    }

    public Vector3 LeftHandPosition
    {
        get
        {
            lock (_lockObject)
            {
                return _solution == null ? new Vector3(0, 0, 0) : _solution.LeftHandPosition;
            }
        }
    }

    public Vector3[] RightHandLandmarks
    {
        get
        {
            lock (_lockObject)
            {
                return _solution == null ? new Vector3[21] : _solution.RightHandLandmarks;
            }
        }
    }

    public Vector3 RightHandPosition
    {
        get
        {
            lock (_lockObject)
            {
                return _solution == null ? new Vector3(0, 0, 0) : _solution.RightHandPosition;
            }
        }
    }

    private MyBootstrap _bootstrap;
    private MyMediaPipeSolution _solution;
    private MyMediaPipeGraph _graph;
    private TextureFramePool _textureFramePool;
    private WebCamSource _webCamSource;

    public MyBootstrap bootstrap
    {
        get
        {
            lock (_lockObject)
            {
                return _bootstrap;
            }
        }
    }

    public MyMediaPipeSolution solution
    {
        get
        {
            lock (_lockObject)
            {
                return _solution;
            }
        }
    }


    public void Awake()
    {
        
        _settingsPrefab = Resources.Load<GameObject>("HandTrackingSettingsPrefabs/MediaPipePluginSettings");
        TextAsset cpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_cpu");
        TextAsset gpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_gpu");
        TextAsset openGlEsConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_opengles");
        _textureFramePool = gameObject.AddComponent<TextureFramePool>();
        _webCamSource = gameObject.AddComponent<WebCamSource>();
        _bootstrap = gameObject.AddComponent<MyBootstrap>();

        initialisationThread = new Thread(() => InitializeDetection(cpuConfig, gpuConfig, openGlEsConfig));
        initialisationThread.Start();
    }

    private void InitializeDetection(TextAsset cpuConfig, TextAsset gpuConfig, TextAsset openGlEsConfig)
    {
        lock (_lockObject)
        {

            UnityEngine.Debug.Log("Initialisation du plugin MediaPipe...");

            _graph = new MyMediaPipeGraph
            {
                _cpuConfig = cpuConfig,
                _gpuConfig = gpuConfig,
                _openGlEsConfig = openGlEsConfig
            };

            _solution = new MyMediaPipeSolution
            {
                runningMode = RunningMode.NonBlockingSync,
                bootstrap = _bootstrap,
                graphRunner = _graph,
                textureFramePool = _textureFramePool,
            };
        }
    }

    public async Task<bool> StartTracking()
    {
        Thread currentThread = Thread.CurrentThread;
        UnityEngine.Debug.Log($"Current thread of play start tracking MediaPipePlugin: {currentThread.Name}");
        lock (_lockObject)
        {
            if (isTracking)
            {
                return false;
            }
            UnityEngine.Debug.Log("Démarrage du plugin MediaPipe...");
            while (_bootstrap.isFinished == false)
            {
                Monitor.Wait(_lockObject, 100);
            }
            _solution.Play();
            _isTracking = true;
            return true;
        }
    }

    public async Task<bool>  StopTracking()
    {
        lock (_lockObject)
        {
            if (!isTracking)
            {
                return false;
            }
            UnityEngine.Debug.Log("Arrêt du plugin MediaPipe...");
            while (_bootstrap.isFinished == false)
            {
                Monitor.Wait(_lockObject, 100);
            }
            _solution.Stop();
            _isTracking = false;
            return true;
        }
    }

    public void SetScreen(Mediapipe.Unity.Screen screen)
    { 
        lock (_lockObject)
        {
            _solution.screen = screen;
            _solution.SetupScreen(ImageSourceProvider.ImageSource);
        }
    }
}
