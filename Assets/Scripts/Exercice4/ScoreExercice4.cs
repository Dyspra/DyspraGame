using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreExercice4 : MonoBehaviour
{
    public int score;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    public void IncreaseScore(int valueToAdd)
    {
        score += valueToAdd;
        text.text = "Score : " + score;
    }
}
