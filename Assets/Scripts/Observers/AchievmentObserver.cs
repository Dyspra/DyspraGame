using UnityEngine;
using Dyspra;

public class AchievmentObserver : AbstractObserver
{
    void Start()
    {
        Debug.Log("AchievmentObserver");
    }

    public override void OnNotify(GameObject entity, E_Event eventToTrigger)
    {
        /*switch (eventToTrigger)
        {
            case E_Event.ACHIEVEMENT_LAUNCHGAME:
                if (entity.name == "Player")
                    Debug.Log("Achievement unlock: " + eventToTrigger);
                break;
            case E_Event.ACHIEVEMENT_QUITGAME:
                if (entity.name == "Player")
                    Debug.Log("Achievement unlock: " + eventToTrigger);
                break;
            default:
                break;
        }*/
    }
}