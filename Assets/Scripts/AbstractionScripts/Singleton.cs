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

        protected virtual void Awake()
        {
            try
            {
                CheckInitInstance();
            }
            catch (Exception e)
            {
                Debug.LogError("Error with singleton class. Exception " + e.Message);
            }
        }

        private void CheckInitInstance()
        {
            if (FindObjectsOfType(typeof(T)).Length > 1)
                throw new Exception("Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(_instance);
                _instance = this as T;
            }
        }
    }

}
