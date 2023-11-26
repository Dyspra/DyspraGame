using UnityEngine;
using System.Collections.Generic;

public class PrefabRespawn : MonoBehaviour
{
    private Vector3[] initialPositions;
    private Rigidbody[] rigidbodies;
    private float changeThreshold;
    private bool resetScheduled;
    private const float canDistanceToGetPoint = 0.1f;
    public Material mat1;
    public Material mat2;
    [SerializeField] private float timeToInvoke = 2.0f;
    private List<Transform> cans = new List<Transform>();

    public MissionChambouleTout mission;

    void Start()
    {
        initialPositions = new Vector3[transform.childCount];
        rigidbodies = new Rigidbody[transform.childCount];
        changeThreshold = transform.childCount * 0.7f;
        resetScheduled = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            initialPositions[i] = transform.GetChild(i).position;
            rigidbodies[i] = transform.GetChild(i).GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        int changedCount = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (Vector3.Distance(transform.GetChild(i).position, initialPositions[i]) > canDistanceToGetPoint)
            {
                if (!cans.Contains(transform.GetChild(i)))
                {
                    cans.Add(transform.GetChild(i));
                    mission.HitACan();
                    transform.GetChild(i).GetComponent<MeshRenderer>().material = mat2;
                }
                changedCount++;
            }
        }

        if (changedCount >= changeThreshold && !resetScheduled)
        {
            Invoke("ResetPyramidPosition", timeToInvoke);
            resetScheduled = true;
        }
    }

    void ResetPyramidPosition()
    {
        resetScheduled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            rigidbodies[i].velocity = Vector3.zero;
            rigidbodies[i].angularVelocity = Vector3.zero;
            transform.GetChild(i).position = initialPositions[i];
            transform.GetChild(i).rotation = Quaternion.identity;
            transform.GetChild(i).GetComponent<MeshRenderer>().material = mat1;
        }
        cans.Clear();
        if (mission != null)
        {
            mission.HitACan();
        }   
    }
}

