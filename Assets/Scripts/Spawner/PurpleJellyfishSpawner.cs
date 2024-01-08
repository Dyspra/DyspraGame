using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleJellyfishSpawner : ObjectSpawner
{
	public float durationBeforeSpawn = 60f;
	public float currentTime;

	// Start is called before the first frame update
	void Start()
	{
		currentTime = durationBeforeSpawn;
	}

	private void Update()
	{
		currentTime -= Time.deltaTime;

		if (currentTime <= 0f)
		{
			currentTime = spawnInSeconds;
			SpawnObjectLeftOfTheScreen(jellyfish);
		}
	}
}
