using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenBall : IBall
{
    public List<GameObject> newListObjectToShoot;
    public float duration = 10.0f;

    private List<GameObject> oldListObjectToShoot;
    private SpawnerBehaviour canon;

    private void Start() 
    {
        canon = canonReference.GetComponent<SpawnerBehaviour>();
    }

    public override void ApplyEffect()
    {
        oldListObjectToShoot = new List<GameObject>(canon.objectToShoot);
        canon.objectToShoot = newListObjectToShoot;
        StartCoroutine(EffectTime(duration));
    }

    IEnumerator EffectTime(float duration)
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(1000, 1000, 1000);
        yield return new WaitForSeconds(duration);
        canon.objectToShoot = oldListObjectToShoot;
        Destroy(this.gameObject);
    }
}
