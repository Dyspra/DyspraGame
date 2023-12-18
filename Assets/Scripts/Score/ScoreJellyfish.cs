using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreJellyfish : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text scoreTextResult;
    public TMP_Text currentScoreText;
    public TMP_Text comboText;
    public int jellyfishLitNumber = 0;
    public int maxJellyfishLit = 0;
    public int score = 0;
    public float duration = 5f;
    public int multiplier = 1;

    void Awake()
    {
        UpdateScoreUI();
        StartCoroutine(GiveScore());
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Méduses : " + jellyfishLitNumber.ToString() + " / " + maxJellyfishLit.ToString();
        }
    }

    public void UpdateScoreResultUI()
    {
        if (scoreTextResult != null)
        {
            scoreTextResult.text = score.ToString();
        }
    }
    public void UpdateJellyfishLit(int valueToAdd)
    {
        jellyfishLitNumber += valueToAdd;
        UpdateMultiplier();
		UpdateScoreUI();
	}

    public void UpdateMaxJellyfishLit(int valueToAdd)
    {
        maxJellyfishLit += valueToAdd;
        UpdateMultiplier();
		UpdateScoreUI();
    }

    IEnumerator GiveScore()
    {
        while (true) 
        {
            yield return new WaitForSeconds(duration);
            score += jellyfishLitNumber * multiplier;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        currentScoreText.text = score.ToString();
    }

    private void UpdateMultiplier()
    {
		float value = ((float)jellyfishLitNumber / (float)maxJellyfishLit) * 100f;
		switch (value)
		{
			case var _ when value >= 90f: //Over 9/10 lit up = *4 bonus
				multiplier = 4;
				comboText.gameObject.SetActive(true);
                comboText.text = "Combo x4 !";
				break;
			case var _ when value >= 66f: //Between 2/3 && 9/10 lit up = *3 bonus
				multiplier = 3;
				comboText.gameObject.SetActive(true);
				comboText.text = "Combo x3 !";
				break;
			case var _ when value >= 33f: //Between 1/3 && 2/3 lit up = *2 bonus
				multiplier = 2;
                comboText.gameObject.SetActive(true);
				comboText.text = "Combo x2 !";
				break;
            case var _ when value < 33f: //Under 1/3 of the jellyfish lit up = no bonus
				multiplier = 1;
                comboText.gameObject.SetActive(false);
                break;
		}
	}
}
