using System.Collections.Generic;
using UnityEngine;

public class GoldenBall : IBall
{
    //public List<GameObject> newListObjectToShoot;

    private SpawnerBehaviour canon;

    private void Start() 
    {
        canon = canonReference.GetComponent<SpawnerBehaviour>();
    }

    public override void ApplyEffect()
    {
        //canon.GoldenListObjectToShoot = new List<GameObject>(canon.objectToShoot);
        //canon.objectToShoot = newListObjectToShoot;
        canon.StartGoldenMode();
        Destroy(this.gameObject);
    }
}
