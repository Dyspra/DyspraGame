using UnityEngine;

namespace Dyspra
{
    // Holds and update UI
    public class LevelObserverBalloonFair : AbstractObserver
    {
        [SerializeField] private AbstractMission mission;
        void Start()
        {
            Debug.Log("Balloon fair observer enable");
        }

        public override void OnNotify(GameObject entity, E_Event eventToTrigger)
        {
            switch (eventToTrigger)
            {
                case E_Event.MISSION_STEP_COMPLETE:
                    mission.LaunchNextEvent();
                    break;
                case E_Event.MISSION_GET_BALLOON:
                    mission.gameObject.GetComponent<MissionBalloonFair>().GetBalloon();
                        Debug.Log("Player get a balloon");
                    break;
                default:
                    Debug.Log("Nothing happened");
                    break;
            }
        }
    }
}
