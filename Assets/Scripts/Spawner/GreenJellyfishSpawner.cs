using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GreenJellyfishSpawner : ObjectSpawner
{
	public int spawnNumberEachTime = 10;
	public float currentTime;
	protected ScoreJellyfish score;

    // Start is called before the first frame update
    void Start()
    {
		score = FindObjectOfType<ScoreJellyfish>(true);
		score.UpdateMaxJellyfishLit(nbOfItems);
		currentTime = spawnInSeconds;
		GetScreenBoundaries();
		delta_time = timer.maxTime;
		for (int i = 0; i < nbOfItems; i++)
		{
			SpawnObject(jellyfish);
		}
	}

	private void Update()
	{
		currentTime -= Time.deltaTime;

		if (currentTime <= 0f)
		{
			currentTime = spawnInSeconds;
			SpawnGreenJellyfish();
		}
	}

	private void SpawnGreenJellyfish()
	{
		for (int i = 0 ; i < spawnNumberEachTime ; i++) 
		{ 
			SpawnObject(jellyfish);
		}
		score.UpdateMaxJellyfishLit(spawnNumberEachTime);
	}
}
