using UnityEngine;

public class MediaPipePythonInterface : MonoBehaviour, IHandTrackingSolution
{
    public string id => "mediapipe-python-interface";
    public string displayName => "MediaPipe Python";
    // Start is called before the first frame update

    private UDPServer _server;
    private PythonProcess _process;

    IEnumerator StartTracking()
    {
        _process = new PythonProcess();
        _server = new UDPServer();
        StartUDPServer();
    }

    IEnumerator StopTracking()
    {
        _process = null;
        _server = null;
        tokenSource.Cancel();
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
