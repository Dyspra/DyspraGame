using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScore : MonoBehaviour
{
    private Score score;

    public int value;

    private void Start() {
        score = GameObject.FindWithTag("ScoreUI").GetComponent<Score>();
    }

    void OnDestroy()
    {
        score.UpdateScore(value);
    }
}
