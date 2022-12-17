using UnityEngine;

public class MoveToSpawn : MonoBehaviour
{
    [SerializeField] private GameObject spawner;

    void Start()
    {
        if (spawner == null)
            Destroy(this);
    }

    void Update()
    {
        if (this.gameObject.transform.position.y < -5)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.transform.position = spawner.transform.position;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
