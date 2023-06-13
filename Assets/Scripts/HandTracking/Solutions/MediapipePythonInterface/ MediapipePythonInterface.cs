using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public class MediaPipePythonInterface : MonoBehaviour, IHandTrackingSolution
{
    public string id => "mediapipe-python-interface";
    public string displayName => "MediaPipe Python";
    // Start is called before the first frame update

    private UDPServer _server;
    private PythonProcess _process;

    private CancellationTokenSource tokenSource;

    public IEnumerator StartTracking()
    {
        _process = new PythonProcess();
        _server = new UDPServer();
        StartUDPServer();
        Task.Run(() => _process.StartProcess());
        yield break;
    }

    public IEnumerator StopTracking()
    {
        _process = null;
        _server = null;
        tokenSource.Cancel();
        yield break;
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
