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

    private MyBootstrap _bootstrap;
    private MyMediaPipeSolution _solution;
    private MyMediaPipeGraph _graph;
    private TextureFramePool _textureFramePool;
    private WebCamSource _webCamSource;

    private void Awake()
    {
        UnityEngine.Debug.Log("Initialisation du plugin MediaPipe...");
        _webCamSource = gameObject.AddComponent<WebCamSource>();
        _bootstrap = gameObject.AddComponent<MyBootstrap>();

        _solution = gameObject.AddComponent<MyMediaPipeSolution>();
        _solution.runningMode = RunningMode.NonBlockingSync;
        _solution.bootstrap = _bootstrap;
        // Find the ScreenMediaPipe object if it exists
        GameObject screenMediaPipe = GameObject.Find("ScreenMediaPipe");
        if (screenMediaPipe != null)
        {
            _solution.screen = screenMediaPipe.GetComponent<Mediapipe.Unity.Screen>();
        }

        _textureFramePool = gameObject.AddComponent<TextureFramePool>();
        _solution.textureFramePool = _textureFramePool;

        _graph = gameObject.AddComponent<MyMediaPipeGraph>();
        _graph._cpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_cpu");
        _graph._gpuConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_gpu");
        _graph._openGlEsConfig = Resources.Load<TextAsset>("CustomMediaPipe/official_hand_tracking_demo_opengles");
        _solution.graphRunner = _graph;
    }
    public Task<bool> StartTracking()
    {
        
        if (isTracking)
        {
            return Task.FromResult(false);
        }
        UnityEngine.Debug.Log("Démarrage du plugin MediaPipe...");
        _isTracking = true;
        return Task.FromResult(true);
    }

    public Task<bool>  StopTracking()
    {
        if (!isTracking)
        {
            return Task.FromResult(false);
        }
        UnityEngine.Debug.Log("Arrêt du plugin MediaPipe...");
        _isTracking = false;
        return Task.FromResult(true);
    }

    // MediaPipe Settings
}
