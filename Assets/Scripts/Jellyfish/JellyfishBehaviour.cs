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
        if (FollowYellowFish)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, YellowFish.position, Time.deltaTime * followSpeed);
            transform.position = newPosition;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "YellowJellyfish") {
            ChangeColor(lighted_mat);
            score += 1;
            FollowYellowFish = true;
        }
        if (other.gameObject.name == "RedJellyfish" && Time.time - hit_by_blue >= timeimmunity) {
            ChangeColor(base_mat);
            score -= 1;
            FollowYellowFish = false;
        }
        if (other.gameObject.name == "BlueJellyfish") {
            ChangeColor(immun_mat);
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
}