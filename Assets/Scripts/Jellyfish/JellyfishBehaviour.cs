using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform YellowFish;
    public float followSpeed = 5f;
    public bool isInvincible = false;
    public float invincibilityDuration = 5f;
    public float hit_by_blue = 0f;
    public float timeimmunity = 7f;
    public bool isFollowingYellowFish = false;
    private Vector2 screenBoundaries;
    private float objectWidth;  
    private float objectHeight;
    public int score = 0;
    public Material base_mat;
    public Material lighted_mat;
    public Material immun_mat;
    public List<Renderer> _renderer = new List<Renderer>();
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,  Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<CapsuleCollider>().radius;
        objectHeight = GetComponent<CapsuleCollider>().height / 2;
        ChangeColor(base_mat);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingYellowFish)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, YellowFish.position, Time.deltaTime * followSpeed);
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
        if (other.gameObject.tag == "Blue") {
            isInvincible = true;
            ChangeColor(immun_mat);
            StartCoroutine(Timer());
        }
        if (other.gameObject.tag == "Green" && isFollowingYellowFish == false && other.gameObject.GetComponent<JellyfishBehaviour>().isFollowingYellowFish == true) {
            score += 1;
            isFollowingYellowFish = true;
        }
    }
    private Vector3 GetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, 0f, randomZ).normalized;
        return randomDir;
    }
    private void ChangeColor(Material mat)
    {
        foreach(Renderer r in _renderer) {
            r.material = mat;
        }
    }
    private void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, screenBoundaries.y * -1 - objectHeight);
        transform.position = viewPos;        
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeimmunity);
        isInvincible = false;
        ChangeColor(lighted_mat);
    }
}