using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TMP_Text scoreText;
    public int score;

    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
        scoreText.text = "Score : " + score;
    }
}
