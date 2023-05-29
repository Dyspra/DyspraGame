using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
