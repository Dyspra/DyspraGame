using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathJellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f; 
    public float changeDirectionInterval = 2f;
    private Vector3 randomDirection;
    private float lastDirectionChangeTime;
    public Material mat;
    public List<Renderer> _renderer = new List<Renderer>();
    void Start()
    {
        foreach(Renderer r in _renderer) {
            r.material = mat;
        }
        randomDirection = GetRandomDirection();
        lastDirectionChangeTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastDirectionChangeTime > changeDirectionInterval)
        {
            randomDirection = GetRandomDirection();
            lastDirectionChangeTime = Time.time;
        }
        Vector3 newPosition = transform.position + randomDirection * moveSpeed * Time.deltaTime;
        if (newPosition.x > 10)
            newPosition.x = 10;
        else if (newPosition.x < -10)
            newPosition.x = -10;
        if (newPosition.y > 7)
            newPosition.y = 7;
        else if (newPosition.x < -7)
            newPosition.x = -7;
        transform.position = newPosition;
    }
    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, randomY, 0f).normalized;
        return randomDir;
    }
}
