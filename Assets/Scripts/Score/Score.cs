using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public int score;

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
        scoreText.text = "Score : " + score;
    }
}
