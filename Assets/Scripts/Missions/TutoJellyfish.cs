using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoJellyfish : MonoBehaviour
{
	public List<GameObject> tutorials;
	public GameObject gameUI;
	public GameObject jellyfishSpawner;
	public GameObject jellyfish;
	int tutoNb = 0;

	private void OnEnable()
	{
		tutorials[tutoNb].SetActive(true);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (tutoNb + 1 < tutorials.Count)
			{
				tutorials[tutoNb].SetActive(false);
				tutoNb++;
				tutorials[tutoNb].SetActive(true);
			} else 
			{
				gameUI.SetActive(true);
				jellyfishSpawner.SetActive(true);
				jellyfish.SetActive(true);
				gameObject.SetActive(false);
			}
		}
	}
}
