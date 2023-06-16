using System.Collections;
using System.Threading.Tasks;

public interface IHandTrackingSolution {
    string id { get; }
    string displayName { get; }

    bool isTracking { get; }

    Task<bool> StartTracking();
    Task<bool> StopTracking();
}
