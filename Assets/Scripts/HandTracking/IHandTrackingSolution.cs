using System.Collections;

public interface IHandTrackingSolution {
    string id { get; }
    string displayName { get; }

    IEnumerator StartTracking();
    IEnumerator StopTracking();
}
