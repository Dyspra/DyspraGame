using UnityEngine;

public class LineSphere : MonoBehaviour
{
    public Transform target = null;
    [SerializeField] private float speed;
    [SerializeField] private float distanceToDestroy = 1.0f;

    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(this.transform.position, target.position) < distanceToDestroy)
                Destroy(this.gameObject);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);

        }
    }
}
