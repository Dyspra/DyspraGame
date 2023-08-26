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
    private Vector3 previousPosition;
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
        if (Time.time - lastDirectionChangeTime > changeDirectionInterval || 
        transform.position.x + objectWidth <= screenBoundaries.x || transform.position.x - objectWidth >= screenBoundaries.x * -1 ||
        transform.position.y + objectHeight <= screenBoundaries.y || transform.position.y - objectHeight >= screenBoundaries.y * -1 )
        {
            randomDirection = GetRandomDirection();
            lastDirectionChangeTime = Time.time;
        }
        previousPosition = transform.position;
        Quaternion rotation = Quaternion.LookRotation(randomDirection, Vector3.up);
        rotation = rotation * Quaternion.Euler(90, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3.0f * Time.deltaTime);
        Vector3 newPosition = transform.position + randomDirection * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
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
        if (previousPosition.x == transform.position.x || previousPosition.y == transform.position.y)
        {
            randomDirection *= -1;
        }
    }
}
