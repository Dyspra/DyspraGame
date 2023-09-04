using UnityEngine;

public class SlowM : MonoBehaviour
{
    private bool isSlowMotionActive = false;
    private float originalTimeScale;

    void Start()
    {
        originalTimeScale = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isSlowMotionActive = !isSlowMotionActive;
            Time.timeScale = isSlowMotionActive ? 0.2f : originalTimeScale;
        }
    }
}



