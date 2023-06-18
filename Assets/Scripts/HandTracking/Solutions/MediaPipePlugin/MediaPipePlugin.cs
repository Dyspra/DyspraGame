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

    public Vector3[] LeftHandLandmarks => _solution.LeftHandLandmarks;
    public Vector3[] RightHandLandmarks => _solution.RightHandLandmarks;

    private MyBootstrap _bootstrap;
    private MyMediaPipeSolution _solution;
    private MyMediaPipeGraph _graph;
    private TextureFramePool _textureFramePool;
    private WebCamSource _webCamSource;

    public MyBootstrap bootstrap => _bootstrap;
    public MyMediaPipeSolution solution => _solution;

    private void Awake()
    {
        _settingsPrefab = Resources.Load<GameObject>("HandTrackingSettingsPrefabs/MediaPipePluginSettings");
    
        UnityEngine.Debug.Log("Initialisation du plugin MediaPipe...");
        _webCamSource = gameObject.AddComponent<WebCamSource>();
        _bootstrap = gameObject.AddComponent<MyBootstrap>();

        _solution = gameObject.AddComponent<MyMediaPipeSolution>();
        _solution.runningMode = RunningMode.NonBlockingSync;
        _solution.bootstrap = _bootstrap;

        _textureFramePool = gameObject.AddComponent<TextureFramePool>();
        _solution.textureFramePool = _textureFramePool;

        _graph = gameObject.AddComponent<MyMediaPipeGraph>();
        _graph._cpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_cpu");
        _graph._gpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_gpu");
        _graph._openGlEsConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_opengles");
        _solution.graphRunner = _graph;
    }
    public async Task<bool> StartTracking()
    {
        if (isTracking)
        {
            return false;
        }
        UnityEngine.Debug.Log("Démarrage du plugin MediaPipe...");
        while (_bootstrap.isFinished == false)
        {
            await Task.Delay(100);
        }
        _solution.Play();
        _isTracking = true;
        return true;
    }

    public async Task<bool>  StopTracking()
    {
        if (!isTracking)
        {
            return false;
        }
        UnityEngine.Debug.Log("Arrêt du plugin MediaPipe...");
        while (_bootstrap.isFinished == false)
        {
            await Task.Delay(100);
        }
        _solution.Stop();
        _isTracking = false;
        return true;
    }

    // MediaPipe Settings

    public void SetScreen(Mediapipe.Unity.Screen screen)
    {
        _solution.screen = screen;
        _solution.SetupScreen(ImageSourceProvider.ImageSource);
    }
}
