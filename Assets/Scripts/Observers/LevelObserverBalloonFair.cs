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
                    if (mission.gameObject.GetComponent<MissionBalloonFairScore>())
                        mission.gameObject.GetComponent<MissionBalloonFairScore>().GetBalloon();
                    else if (mission.gameObject.GetComponent<MissionBalloonFairTimer>())
                        mission.gameObject.GetComponent<MissionBalloonFairTimer>().GetBalloon();
                    Debug.Log("Player get a balloon");
                    break;
                default:
                    Debug.Log("Nothing happened");
                    break;
            }
        }
    }
}
