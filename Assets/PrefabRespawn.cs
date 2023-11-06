using UnityEngine;

public class PrefabRespawn : MonoBehaviour
{
    private Vector3[] initialPositions;
    private Rigidbody[] rigidbodies;
    private float changeThreshold;
    private bool resetScheduled;

    public ScoreManager scoreManager;

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
            if (Vector3.Distance(transform.GetChild(i).position, initialPositions[i]) > 0.01f)
            {
                changedCount++;
            }
        }

        if (changedCount >= changeThreshold && !resetScheduled)
        {
            Invoke("ResetPyramidPosition", 2f);
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
        }

        if (scoreManager != null)
        {
            scoreManager.IncreaseScore();
        }   
    }
}

