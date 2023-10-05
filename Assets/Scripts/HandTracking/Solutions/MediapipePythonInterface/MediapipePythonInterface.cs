using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class MediaPipePythonInterface : MonoBehaviour, IHandTrackingSolution
{
    public string id => "mediapipe-python-interface";
    public string displayName => "MediaPipe Python";

    public bool isTracking => _server != null && _process != null;
    
    public GameObject settingsPrefab => _settingsPrefab;
    private GameObject _settingsPrefab = null;

    // public List<Vector3> handLandmarks 
    // {
    //     get 
    //     {
    //         if (_server == null)
    //         {
    //             return new List<Vector3>();
    //         }
    //         return _server.HandsPosition.packages.ConvertAll(v => new Vector3(v.position.x, v.position.y, v.position.z));
    //     }
    // }
    private Vector3[] _leftHandLandmarks = new Vector3[21];
    private Vector3[] _rightHandLandmarks = new Vector3[21];

// _server.HandsPosition.packages.ConvertAll(v => new Vector3(v.position.x, v.position.y, v.position.z));
    public Vector3[] LeftHandLandmarks 
    {
        get 
        {
            if (_server == null || _server.HandsPosition.packages.Count < 21)
            {
                return _leftHandLandmarks;
            }
            var landmarks = _server.HandsPosition.packages;
            for (int i = 0; i < 21; i++)
            {
                _leftHandLandmarks[i] = new Vector3(landmarks[i].position.x, landmarks[i].position.y, landmarks[i].position.z);
            }
            return _leftHandLandmarks;
        }
    }
    public Vector3 LeftHandPosition
    {
        get
        {
            if (_server == null || _server.HandsPosition.packages.Count < 21)
            {
                return Vector3.zero;
            }
            var landmarks = _server.HandsPosition.packages;
            return new Vector3(landmarks[0].position.x, landmarks[0].position.y, landmarks[0].position.z);
        }
    }
    public Vector3[] RightHandLandmarks 
    {
        get 
        {
            if (_server == null || _server.HandsPosition.packages.Count < 42)
            {
                return _rightHandLandmarks;
            }
            var landmarks = _server.HandsPosition.packages;
            for (int i = 21; i < 42; i++)
            {
                _rightHandLandmarks[i - 21] = new Vector3(landmarks[i].position.x, landmarks[i].position.y, landmarks[i].position.z);
            }
            return _rightHandLandmarks;
        }
    }
    public Vector3 RightHandPosition
    {
        get
        {
            if (_server == null || _server.HandsPosition.packages.Count < 42)
            {
                return Vector3.zero;
            }
            var landmarks = _server.HandsPosition.packages;
            return new Vector3(landmarks[21].position.x, landmarks[21].position.y, landmarks[21].position.z);
        }
    }

    //todo: make this private when everything is working
    public UDPServer _server;
    private PythonProcess _process;
    public PythonProcess process => _process;

    private CancellationTokenSource tokenSource;

    private void Awake()
    {
        _settingsPrefab = Resources.Load<GameObject>("HandTrackingSettingsPrefabs/MediapipePythonInterfaceSettings");
    }
    public Task<bool> StartTracking()
    {
        UnityEngine.Debug.Log("Démarrage du process Python...");
        _process = new PythonProcess();
        _server = new UDPServer();
        StartUDPServer();
        _process.StartProcess();
        return Task.FromResult(true);
    }
    public Task<bool> StopTracking()
    {
        UnityEngine.Debug.Log("Arrêt du process Python...");
        if (_process != null)
        {
            _process.StopProcess();
        }
        _process = null;
        _server = null;
        tokenSource.Cancel();
        return Task.FromResult(true);
    }

    void StartUDPServer()
    {
        UnityEngine.Debug.Log("Démarrage du serveur UDP...");
        tokenSource = new CancellationTokenSource();
        _server.Initialize();
        _server.StartMessageLoop(tokenSource.Token);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
