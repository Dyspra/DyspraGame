using UnityEngine;

namespace Dyspra
{
    public class LevelObserver : AbstractObserver
    {
        [SerializeField] private AbstractMission mission;
        void Start()
        {
            Debug.Log("LevelObserver");
        }
    
        public override void OnNotify(GameObject entity, E_Event eventToTrigger)
        {
            switch (eventToTrigger)
            {
                case E_Event.MISSION_STEP_COMPLETE:
                    mission.LaunchNextEvent();
                    break;
                case E_Event.LEVELEVENT_GET_KEY:
                    if (entity.tag == "Player")
                        Debug.Log("Player get a key");
                    break;
                case E_Event.LEVELEVENT_OPEN_DOOR:
                    if (entity.tag == "Player")
                        Debug.Log("Door opened");
                    break;
                default:
                    Debug.Log("Nothing happened");
                    break;
            }
        }
    }
}
