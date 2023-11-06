using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer laserRender;
    public Transform[] points;
    void Start()
    {
//        laserRender  = GetComponent<LineRenderer>();
    }
    
    public void SetUpLine(Transform[] points) {
        laserRender.positionCount = points.Length;
        this.points = points;
    }
    
    private void Update() {
        for (int i = 0; i < points.Length; i++) {
            laserRender.SetPosition(i, points[i].position);
        }
    }
}
