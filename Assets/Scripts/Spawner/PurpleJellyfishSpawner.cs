using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleJellyfishSpawner : ObjectSpawner
{
	public float durationBeforeSpawn = 60f;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(SpawnRedJellyfish());
	}

	IEnumerator SpawnRedJellyfish()
	{
		yield return new WaitForSeconds(durationBeforeSpawn);
		while (true)
		{
			SpawnObjectLeftOfTheScreen(jellyfish);
			yield return new WaitForSeconds(spawnInSeconds);
		}
	}
}
