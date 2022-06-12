using UnityEngine;
using System;

namespace Dyspra
{
    /// <summary>
    /// Base class that shouldn't be instantiate twice
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        [SerializeField] private bool shouldBeDestroyOnLoad = false; // If false, instance will be keep throughout all scenes

        protected virtual void Awake()
        {
            try
            {
                CheckInitInstance();
            }
            catch (Exception e)
            {
                Debug.LogError("Error with singleton class. Exception: " + e.Message);
            }
        }

        private void CheckInitInstance()
        {
            if (FindObjectsOfType(typeof(T)).Length > 2)
                throw new Exception("Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
            if (_instance == null)
            {
                _instance = this as T;
                if (shouldBeDestroyOnLoad == false)
                    DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(_instance.gameObject);
                _instance = this as T;
                Debug.Log("<color=yellow>Warning:</color> you've just " + 
                    "instantiate a singleton twice. The first one has been destroyed");
            }
        }
    }

}
