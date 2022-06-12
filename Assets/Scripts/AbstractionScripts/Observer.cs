using UnityEngine;

namespace Dyspra
{
    /// <summary>
    /// Singleton class made to wait for event to be triggered
    /// </summary>
    public class Observer : Singleton<Observer>
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("kek");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
