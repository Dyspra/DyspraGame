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
            /*switch (eventToTrigger)
            {
                case E_Event.MISSION_STEP_COMPLETE:
                    mission.LaunchNextEvent();
                    break;
                case E_Event.MISSION_GET_BALLOON:
                    if (entity.tag == "Player")
                        Debug.Log("Player get a key");
                    break;
                default:
                    Debug.Log("Nothing happened");
                    break;
            }*/
        }
    }
}
