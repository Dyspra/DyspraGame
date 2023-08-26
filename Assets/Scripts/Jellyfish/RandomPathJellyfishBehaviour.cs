using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPathJellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    private Vector2 screenBoundaries;
    private float objectWidth;  
    private float objectHeight;
    public float changeDirectionInterval = 2f;
    private Vector3 randomDirection;
    private float lastDirectionChangeTime;
    public Material mat;
    public List<Renderer> _renderer = new List<Renderer>();
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,  Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<CapsuleCollider>().radius;
        objectHeight = GetComponent<CapsuleCollider>().height / 2;
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
        transform.position = newPosition;
    }
    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, randomY, 0f).normalized;
        return randomDir;
    }
    private void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, (screenBoundaries.y * -1) - objectHeight);
        transform.position = viewPos;        
    }
}
