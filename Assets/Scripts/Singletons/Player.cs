using UnityEngine;
using Dyspra;

public class Player : Singleton<Player>, ISubject
{
    private Subject _subject = new Subject();
    private AbstractCommand _command = null;
    private InputHandler _inputHandler = new InputHandler();

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
        _command = _inputHandler.HandleInput();
        if (_command != null)
            _command.Execute(this.gameObject);
    }

    #region Fake Inpute
    public void BackInMenu()
    {
        Debug.Log("Player move back in menu");
    }

    public void ValidateInMenu()
    {
        Debug.Log("Player validate a menu");
    }

    public void Teleport()
    {
        Debug.Log("Player teleported");
    }
    #endregion

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
