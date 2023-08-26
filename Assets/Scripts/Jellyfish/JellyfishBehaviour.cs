using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform YellowFish;
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 2f;
    public float followSpeed = 5f;
    public bool isInvincible = false;
    public float invincibilityDuration = 5f;
    public bool isFollowingYellowFish = false;
    private Vector2 screenBoundaries;
    private float objectWidth;  
    private float objectHeight;
    public int score = 0;
    public Material base_mat;
    public Material lighted_mat;
    public Material immun_mat;
    public List<Renderer> _renderer = new List<Renderer>();
    private Vector3 randomDirection;
    private float lastDirectionChangeTime;
    private Vector3 previousPosition;
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,  Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<CapsuleCollider>().radius;
        objectHeight = GetComponent<CapsuleCollider>().height / 2;
        ChangeColor(base_mat);
        randomDirection = GetRandomDirection();
        lastDirectionChangeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingYellowFish)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, YellowFish.position, Time.deltaTime * followSpeed);
            transform.position = newPosition;
        } else {
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
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Yellow") {
            ChangeColor(lighted_mat);
            score += 1;
            isFollowingYellowFish = true;
        }
        if (other.gameObject.tag == "Red" && isInvincible == false && isFollowingYellowFish == true) {
            ChangeColor(base_mat);
            score -= 1;
            isFollowingYellowFish = false;
        }
        if (other.gameObject.tag == "Blue" && isFollowingYellowFish == true) {
            isInvincible = true;
            ChangeColor(immun_mat);
            StartCoroutine(Timer());
        }
        if (other.gameObject.tag == "Green" && isFollowingYellowFish == false && other.gameObject.GetComponent<JellyfishBehaviour>().isFollowingYellowFish == true) {
            score += 1;
            isFollowingYellowFish = true;
        }
    }

    private void ChangeColor(Material mat)
    {
        foreach(Renderer r in _renderer) {
            r.material = mat;
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        ChangeColor(lighted_mat);
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
            lastDirectionChangeTime = Time.time;
        }
    }
}