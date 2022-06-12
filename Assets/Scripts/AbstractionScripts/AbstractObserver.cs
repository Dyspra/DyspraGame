using UnityEngine;

namespace Dyspra
{
    /// <summary>
    /// Class made to wait for event to be triggered by object
    /// </summary>
    public abstract class AbstractObserver : MonoBehaviour
    {
        abstract public void OnNotify(GameObject entity, E_Event eventToTrigger);
    }

    /// <summary>
    /// Enum holding all possible event to trigger
    /// </summary>
    public enum E_Event
    {
        ACHIEVEMENT_LAUNCHGAME,
        ACHIEVEMENT_QUITGAME,
        LEVELEVENT_GET_KEY,
        LEVELEVENT_OPEN_DOOR
    }
}
