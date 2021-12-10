using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManaging : MonoBehaviour
{
    public Image fondu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fondu.color.a > 0) {
            fondu.color = new Color(fondu.color.r, fondu.color.g, fondu.color.b, fondu.color.a - 0.03f);
            if (fondu.color.a <= 0)
                fondu.enabled = false;
        }
    }
}
