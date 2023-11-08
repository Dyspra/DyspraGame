using UnityEngine;

public abstract class IBall : MonoBehaviour, Dyspra.ISubject
{
    public GameObject canonReference;
    public float spawnProbability;
    private Dyspra.Subject _subject = new Dyspra.Subject();
    private Rigidbody _body;
    private Vector3 velocity;

    public abstract void ApplyEffect();

	private void Awake()
	{
        _body = GetComponent<Rigidbody>();
		GameStateManager.Instance.onGameStateChange += OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameStateManager.Instance.onGameStateChange -= OnGameStateChanged;
	}

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ApplyEffect();
        }
    }

	private void OnGameStateChanged(GameState newGameState)
	{
		enabled = newGameState == GameState.Gameplay;
        if (newGameState == GameState.Paused)
        {
            velocity = _body.velocity;
            _body.constraints = RigidbodyConstraints.FreezeAll;
        } else
        {
            _body.constraints = RigidbodyConstraints.None;
            _body.velocity = velocity;
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
