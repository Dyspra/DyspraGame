using UnityEngine;

public abstract class IBall : MonoBehaviour, Dyspra.ISubject
{
    public GameObject canonReference;
    public float spawnProbability;
    private Dyspra.Subject _subject = new Dyspra.Subject();

    public abstract void ApplyEffect();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ApplyEffect();
        }
    }

    #region Subject initialization
    public void NotifyObservers(GameObject entity, Dyspra.E_Event eventToTrigger)
    {
        _subject.NotifyObservers(entity, eventToTrigger);
    }

    public void AddObserver(ref Dyspra.AbstractObserver observer)
    {
        _subject.AddObserver(ref observer);
    }

    public void RemoveObserver(ref Dyspra.AbstractObserver observer)
    {
        _subject.RemoveObserver(ref observer);
    }
    #endregion
}
