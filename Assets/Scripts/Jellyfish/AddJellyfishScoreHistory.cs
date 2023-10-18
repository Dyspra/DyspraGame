using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AddJellyfishScoreHistory : MonoBehaviour
{
    bool exoDone = false;
    ScoreJellyfish scoreJellyfish;
    Timer timer;

    void Start()
    {
        scoreJellyfish = GetComponent<ScoreJellyfish>();
        timer = GetComponent<Timer>();
    }

    void Update()
    {
        if (!exoDone && timer.currentTime <= 0f)
        {
            exoDone = true;
            BDDInteractor.Instance.AddHistory("2", scoreJellyfish.score);
        }
    }
}
