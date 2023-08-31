using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowJellyfishBehaviour : AJellyfishBehaviour
{
    protected override void Start()
    {
        GetScreenBoundaries();
        isLightUp = true;
    }
    protected override void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundaries.x + objectWidth, screenBoundaries.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundaries.y + objectHeight, (screenBoundaries.y * -1) - objectHeight);
        viewPos.z = 0f;
        transform.position = viewPos;
    }

}
