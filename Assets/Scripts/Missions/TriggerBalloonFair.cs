using UnityEngine;
using Dyspra;

// The balloon fair trigger notify when player gets a balloon or trigger another mission step
public class TriggerBalloonFair : MonoBehaviour, ISubject
{
    private Subject _subject = new Subject();


    void Start()
    {
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetBalloon();
        }
    }

    void GetBalloon()
    {
        NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_GET_BALLOON);
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
