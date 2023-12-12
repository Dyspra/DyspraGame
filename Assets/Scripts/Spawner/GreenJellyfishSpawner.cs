using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenJellyfishSpawner : ObjectSpawner
{
	public int spawnNumberEachTime = 10;
	protected ScoreJellyfish score;

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(SpawnGreenJellyfish());
		score = FindObjectOfType<ScoreJellyfish>(true);
		score.UpdateMaxJellyfishLit(nbOfItems);
	}

	private void OnEnable()
	{
		GetScreenBoundaries();
		delta_time = timer.maxTime;
		for (int i = 0; i < nbOfItems; i++)
		{
			SpawnObject(jellyfish);
		}
	}

	IEnumerator SpawnGreenJellyfish()
	{
		while (true) 
		{
			yield return new WaitForSeconds(spawnInSeconds);
			for (int i = 0 ; i < spawnNumberEachTime ; i++) 
			{ 
				SpawnObject(jellyfish);
			}
			score.UpdateMaxJellyfishLit(spawnNumberEachTime);
		}
	}
}
