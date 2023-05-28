using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public float rotationSpeed = 1;
    [Range(1, 100)] public int blastPower = 5;

    public List<GameObject> objectToShoot;
    public Transform shotPoint;
    public bool displayLine = true;

    public float durationBetweenShots = 1f;
    private float timePassed = 0f;
    public float totalWeight = 0;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        UpdateWeight();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blastPower < 0)
            blastPower = 1;
        if (displayLine && !line.enabled)
            line.enabled = true;
        else if (!displayLine && line.enabled)
            line.enabled = false;
        timePassed += Time.deltaTime;
        if (timePassed > durationBetweenShots)
        {
            timePassed = 0;
            float diceRoll = Random.Range(0f, totalWeight);
            foreach(GameObject item in objectToShoot)
            {
                IBall ball = item.GetComponent<IBall>();
                if (ball.spawnProbability >= diceRoll)
                {
                    GameObject createdObject = Instantiate(item, shotPoint.position, shotPoint.rotation);
                    createdObject.GetComponent<Rigidbody>().velocity = shotPoint.transform.up * blastPower;
                    createdObject.GetComponent<IBall>().canonReference = this.gameObject;
                    break;
                }
                diceRoll -= ball.spawnProbability;
            }
        }
    }

    public void UpdateWeight()
    {
        totalWeight = objectToShoot.Sum(item => item.GetComponent<IBall>().spawnProbability);
    }
}
