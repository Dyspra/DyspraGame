using UnityEngine;
using Dyspra;

public class Player : Singleton<Player>, ISubject
{
    private Subject _subject = new Subject();

    void Start()
    {
        AbstractObserver[] oui = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < oui.Length; i++)
            AddObserver(ref oui[i]);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            NotifyObservers(this.gameObject, E_Event.LEVELEVENT_GET_KEY);
            Debug.Log("Key pressed");
        }
    }

    #region Subject initialization
    public void NotifyObservers(GameObject entity, E_Event eventToTrigger)
    {
        _subject.NotifyObservers(entity, eventToTrigger);
    }

    public void AddObserver(ref AbstractObserver observer)
    {
        _subject.AddObserver(ref observer);
    }

    public void RemoveObserver(ref AbstractObserver observer)
    {
        _subject.RemoveObserver(ref observer);
    }
    #endregion
}
