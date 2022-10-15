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
        // Mission events
        MISSION_STEP_COMPLETE,
        LEVELEVENT_OPEN_DOOR,
        LEVELEVENT_GET_KEY,

        // Achievmement event
        ACHIEVEMENT_QUITGAME,
        ACHIEVEMENT_LAUNCHGAME

        
    }
}
