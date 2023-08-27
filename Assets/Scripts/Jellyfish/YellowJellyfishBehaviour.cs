using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowJellyfishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform LaserDirection;
    public float followSpeed = 5f;
    private Vector2 screenBoundaries;
    private float objectWidth;  
    private float objectHeight;    
    public Material yellow_mat;
    public List<Renderer> _renderer = new List<Renderer>();
    void Start()
    {
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,  Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<CapsuleCollider>().radius;
        objectHeight = GetComponent<CapsuleCollider>().height / 2;
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
    private void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, screenBoundaries.y * -1 - objectHeight);
        transform.position = viewPos;        
    }
}
