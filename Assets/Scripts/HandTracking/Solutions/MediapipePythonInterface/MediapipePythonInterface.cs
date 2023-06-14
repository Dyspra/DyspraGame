using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public class MediaPipePythonInterface : IHandTrackingSolution
{
    public string id => "mediapipe-python-interface";
    public string displayName => "MediaPipe Python";
    // Start is called before the first frame update

    //todo: make this private when everything is working
    public UDPServer _server;
    private PythonProcess _process;

    private CancellationTokenSource tokenSource;

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
