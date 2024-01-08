using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	public GameObject jellyfish;
	public float spawnInSeconds = 60f;
	protected Vector2 screenBoundaries;
    protected float[] jellyfish_select = new float[3];
    public int nbOfItems = 0;
    public Timer timer;
    [SerializeField] protected int initialItemSpawn = 0;
    protected float select;
    protected float delta_time = 0;

	protected bool isInitiallyActive;
	protected bool isPaused = false;

	void Awake()
	{
		GameStateManager.Instance.onGameStateChange += OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameStateManager.Instance.onGameStateChange -= OnGameStateChanged;
	}

	private void OnEnable()
	{
		GetScreenBoundaries();
	}

	protected void GetScreenBoundaries()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }
    protected void SpawnObject(GameObject ToSpawn)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(screenBoundaries.x * -1, screenBoundaries.x),
            Random.Range(screenBoundaries.y * -1, screenBoundaries.y),
            0f
        );
        Instantiate(ToSpawn, randomPosition, Quaternion.identity);
    }

    protected void SpawnObjectBottomOfTheScreen(GameObject ToSpawn)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(screenBoundaries.x * -1, screenBoundaries.x),
            screenBoundaries.y,
            0f
            );
        Instantiate(ToSpawn, randomPosition, Quaternion.identity);
    }

	protected void SpawnObjectLeftOfTheScreen(GameObject ToSpawn)
	{
		Vector3 randomPosition = new Vector3(
			screenBoundaries.x,
			Random.Range(screenBoundaries.y * -1, screenBoundaries.y),
			0f
			);
		Instantiate(ToSpawn, randomPosition, Quaternion.identity);
	}

	private void OnGameStateChanged(GameState newGameState)
	{
		if (newGameState == GameState.Gameplay && isInitiallyActive == true)
		{
			isPaused = false;
			enabled = true;
		}
		else if (newGameState == GameState.Paused)
		{
			isInitiallyActive = enabled;
			isPaused = true;
			enabled = false;
		}
	}

}
