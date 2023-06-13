using UnityEngine;
using System.Collections;

public class MediaPipePlugin : MonoBehaviour, IHandTrackingSolution
{
    public string id => "mediapipe-plugin";
    public string displayName => "MediaPipe Plugin";
    // Start is called before the first frame update
    
    public IEnumerator StartTracking()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator StopTracking()
    {
        throw new System.NotImplementedException();
    }
}
