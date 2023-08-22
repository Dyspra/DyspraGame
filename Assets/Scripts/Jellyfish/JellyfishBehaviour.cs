using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform YellowFish;
    public float followSpeed = 5f;
    public float hit_by_blue = 0f;
    public float timeimmunity = 7f;
    public bool FollowYellowFish = false;
    public int score = 0;
    void Start()
    {
        //gameObject.GetComponent().material.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowYellowFish)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, YellowFish.position, Time.deltaTime * followSpeed);
            transform.position = newPosition;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "YellowJellyfish") {
            //gameObject.GetComponent().material.color = Color.green;
            score += 1;
            FollowYellowFish = true;
        }
        if (other.gameObject.name == "RedJellyfish" && Time.time - hit_by_blue >= timeimmunity) {
            //gameObject.GetComponent().material.color = Color.grey;
            score -= 1;
            FollowYellowFish = false;
        }
        if (other.gameObject.name == "BlueJellyfish") {
            //gameObject.GetComponent().material.color = Color.green;
            score += 1;
            FollowYellowFish = true;
            hit_by_blue = Time.time;
        }
    }
    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, 0f, randomZ).normalized;
        return randomDir;
    }
}
