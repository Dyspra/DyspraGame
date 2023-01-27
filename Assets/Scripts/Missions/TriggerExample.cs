using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dyspra;

public class TriggerExample : MonoBehaviour, ISubject
{
    // Start is called before the first frame update
    private Subject _subject = new Subject();
    public int sphereDestroyed = 0;
    void Start()
    {
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("SPHERE DESTROYED");
        Destroy(col.gameObject);
        sphereDestroyed++;
        if (sphereDestroyed == 1)
            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
        else if (sphereDestroyed == 4)
            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
        else if (sphereDestroyed == 5)
            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);

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
