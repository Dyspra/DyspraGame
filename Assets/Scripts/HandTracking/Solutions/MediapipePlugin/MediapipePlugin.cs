using UnityEngine;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

public class MediaPipePlugin : IHandTrackingSolution
{
    public string id => "mediapipe-plugin";
    public string displayName => "MediaPipe Plugin";
    // Start is called before the first frame update
    
    public Task<bool>  StartTracking()
    {
        UnityEngine.Debug.Log("Démarrage du plugin MediaPipe...");
        return Task.FromResult(true);
    }

    public Task<bool>  StopTracking()
    {
        UnityEngine.Debug.Log("Arrêt du plugin MediaPipe...");
        return Task.FromResult(true);
    }
}
