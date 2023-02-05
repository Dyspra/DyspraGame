using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace Dyspra
{
    /// <summary>
    /// Class that holds and triggers each events of a mission
    /// </summary>
    public abstract class AbstractMission : MonoBehaviour
    {
        [SerializeField] private List<UnityEvent> missionEventList = new List<UnityEvent>();
        private int missionIndex = 1;

        public void LaunchNextEvent()
        {
            if (missionIndex < missionEventList.Count && missionEventList.Count > 0)
                missionEventList[missionIndex].Invoke();
        }

        protected void MissionEventComplete()
        {
            missionIndex++;
        }
    }
}
