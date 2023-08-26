using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowJellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform LaserDirection;
    public float followSpeed = 5f;
    public float hit_by_blue = 0f;
    public float blueimmunity = 7f;
    public bool FollowYellowFish = false;
    public Material yellow_mat;
    public List<Renderer> _renderer = new List<Renderer>();
    void Start()
    {
        foreach(Renderer r in _renderer) {
            r.material = yellow_mat;
        }
    }

    // Update is called once per frame
    void Update()
    {
            Vector3 newPosition = Vector3.Lerp(transform.position, LaserDirection.position, Time.deltaTime * followSpeed);
            transform.position = newPosition;
    }

}
