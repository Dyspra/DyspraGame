using UnityEngine;

public class LineSphere : MonoBehaviour
{
    public Transform target = null;
    [SerializeField] private float speed;
    [SerializeField] private float distanceToDestroy = 1.0f;

    public GameObject _initialPos;

    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(this.transform.position, target.position) < distanceToDestroy && _initialPos != null)  
                this.transform.position = new Vector3(_initialPos.transform.position.x, _initialPos.transform.position.y, _initialPos.transform.position.z + 0.5f);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);

        }
    }
}
