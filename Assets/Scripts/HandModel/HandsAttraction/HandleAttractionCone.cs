using UnityEngine;

public class HandleAttractionCone : MonoBehaviour
{
    MeshCollider coneCollider;
    [SerializeField] AttractToTarget AttractToTarget;
    private void Awake()
    {
        coneCollider = GetComponent<MeshCollider>();
        AttractToTarget = FindObjectOfType<AttractToTarget>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter trigger " + other.name);
        if (other.gameObject.name != "hand_2" && other.gameObject.TryGetComponent<Rigidbody>(out var rB) && other.tag != "Player")
            AttractToTarget.SetRigidbodyAttracted(rB);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit trigger " + other.name);
        if (other.gameObject.name != "hand_2" && other.gameObject.TryGetComponent<Rigidbody>(out var rB) && other.tag != "Player")
            AttractToTarget.RemoveRigidbodyAttracted(rB);
    }

    public void SetTarget(AttractToTarget newTarget)
    {
        AttractToTarget = newTarget;
    }
}
