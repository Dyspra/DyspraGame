using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AJellyfishBehaviour : MonoBehaviour
{
    [HideInInspector] public Transform laserDirection;
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 2f;
    public float followSpeed = 5f;
    public bool isInvincible = false;
    public float invincibilityDuration = 5f;
    public bool isLightUp = false;
    protected Vector2 screenBoundaries;
    protected int ToAdd = 0; 
    protected float objectWidth;  
    protected float objectHeight;
    public Material base_mat;
    public List<Renderer> _renderer = new List<Renderer>();
    protected Vector3 randomDirection;
    protected float lastDirectionChangeTime;
    protected Vector3 previousPosition;
    public ScoreJellyfish score;
    protected virtual void Start()
    {
        laserDirection = GameObject.FindWithTag("LaserDirection").transform;
        GetScreenBoundaries();
        foreach(Renderer r in _renderer) {
            r.material = base_mat;
        }
        randomDirection = GetRandomDirection();
        lastDirectionChangeTime = Time.time;
    }

    protected void GetScreenBoundaries()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,  Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<CapsuleCollider>().radius;
        objectHeight = GetComponent<CapsuleCollider>().height / 2;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (isLightUp)
        {
            Move(laserDirection.position);
        } else {
            RandomMove();
        }
    }
    protected void RandomMove()
    {
        if (Time.time - lastDirectionChangeTime > changeDirectionInterval || 
                transform.position.x + objectWidth <= screenBoundaries.x || transform.position.x - objectWidth >= screenBoundaries.x * -1 ||
                transform.position.y + objectHeight <= screenBoundaries.y || transform.position.y - objectHeight >= screenBoundaries.y * -1 )
        {
            randomDirection = GetRandomDirection();
            lastDirectionChangeTime = Time.time;
        }
        Move(randomDirection);
    }

    private void Move(Vector3 direction)
    {
        previousPosition = transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        rotation = rotation * Quaternion.Euler(90, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 3.0f * Time.deltaTime);
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        transform.position = newPosition;
    }

    protected Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, randomY, 0f).normalized;

        return randomDir;
    }

    protected virtual void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, (screenBoundaries.y * -1) - objectHeight);
        transform.position = viewPos;
        if (previousPosition.x == transform.position.x || previousPosition.y == transform.position.y)
        {
            randomDirection *= -1;
            lastDirectionChangeTime = Time.time;
        }
    }
}