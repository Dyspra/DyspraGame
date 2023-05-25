using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    public float rotationSpeed = 1;
    [Range(1, 100)] public int blastPower = 5;

    public List<GameObject> goldenListObjectToShoot;
    public List<GameObject> normalListObjectToShoot;
    public List<GameObject> objectToShoot;
    public Transform shotPoint;
    public bool displayLine = true;

    private bool isGolden = false;

    public float durationBetweenShots = 1f;
    public float totalWeight = 0;
    public float goldenDuration = 3.0f;
    public float goldenShotCooldown = 0.4f;
    private float timePassed = 0f;
    private float totalTimer = 0f;

    LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        timePassed = durationBetweenShots + 1.0f;
        objectToShoot = normalListObjectToShoot;
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
                    RegularBall regBall = createdObject.GetComponent<RegularBall>();

                    if (regBall != null && isGolden)
                    {
                        regBall.StartGolden(totalTimer);
                    }

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

    public IEnumerator EffectTime()
    {
        isGolden = true;
        float oldDurationBetweenShot = durationBetweenShots;
        durationBetweenShots = goldenShotCooldown;
        timePassed = 2;
        objectToShoot = goldenListObjectToShoot;

        totalTimer = goldenDuration;

        while (totalTimer > 0)
        {
            totalTimer -= Time.deltaTime;
            yield return null;
        }

        totalTimer = goldenDuration;

        objectToShoot = normalListObjectToShoot;
        durationBetweenShots = oldDurationBetweenShot;
        timePassed = 0;
        isGolden = false;
    }

    public void StartGoldenMode()
    {
        if (isGolden == false)
            StartCoroutine(EffectTime());
    }
}
