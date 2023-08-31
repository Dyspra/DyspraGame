using UnityEngine;

/**
 * This abstract class is a template for creating singleton game objects in Unity.
 * It ensures that only one instance of the object is created and provides a way to access it globally.
 * 
 * @typeparam T The type of the singleton object.
 */
public abstract class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    private static void CreateInstance()
    {
        m_Instance= (T)FindObjectOfType(typeof(T));
        if (m_Instance == null)
        {
            var singletonObject = new GameObject();
            m_Instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).ToString() + " (Singleton)";
            DontDestroyOnLoad(singletonObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    UnityEngine.Debug.Log("Creating new instance of type: " + typeof(T));
                    CreateInstance();
                }
                return m_Instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}
