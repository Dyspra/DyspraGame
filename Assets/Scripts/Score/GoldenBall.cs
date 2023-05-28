using System.Collections.Generic;
using UnityEngine;

public class GoldenBall : IBall
{
    //public List<GameObject> newListObjectToShoot;

    private SpawnerBehaviour canon;
    private AudioSource audioSource;

    private void Start() 
    {
        canon = canonReference.GetComponent<SpawnerBehaviour>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void ApplyEffect()
    {
        SpawnerBehaviour[] spwnFounded = FindObjectsOfType<SpawnerBehaviour>();


        for (int i = 0; i < spwnFounded.Length; i++)
        {
            Debug.Log(spwnFounded[i].gameObject.name);
            if (spwnFounded[i].enabled == true)
                spwnFounded[i].StartGoldenMode();
        }
        AudioSource.PlayClipAtPoint(audioSource.clip, this.transform.position);
        Destroy(this.gameObject);
    }
}
