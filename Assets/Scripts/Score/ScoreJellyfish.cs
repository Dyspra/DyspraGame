using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreJellyfish : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text scoreTextResult;
    public int score = 0;

    void Start()
    {
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
    public void UpdateScoreResultUI()
    {
        if (scoreTextResult != null)
        {
            scoreTextResult.text = score.ToString();
        }
    }
    public void UpdateScore(int valueToAdd)
    {
        score += valueToAdd;
        UpdateScoreUI();
    }
}
