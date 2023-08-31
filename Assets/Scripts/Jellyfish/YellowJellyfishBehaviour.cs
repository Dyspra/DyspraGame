using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowJellyfishBehaviour : AJellyfishBehaviour
{
    // Start is called before the first frame update   
    public Material yellow_mat;
    protected override void Start()
    {
        GetScreenBoundaries();
        isLightUp = true;
        foreach(Renderer r in _renderer) {
            r.material = yellow_mat;
        }
    }
    protected override void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, (screenBoundaries.y * -1) - objectHeight);
        transform.position = viewPos;
    }

}
