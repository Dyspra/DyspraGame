using UnityEngine;
using Dyspra;

/// <summary>
/// Fake Player fo testing
/// </summary>
public class Player : Singleton<Player>, ISubject
{
    private Subject _subject = new Subject();
    private AbstractCommand _command = null;
    private InputHandler _inputHandler = new InputHandler();

    void Start()
    {
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Key pressed");
        }
        _command = _inputHandler.HandleInput();
        if (_command != null)
            _command.Execute(this.gameObject);
    }

    #region Fake Input
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