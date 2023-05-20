using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBall : IBall
{
    public int scoreModifier = 1;

    private Score target;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Score>();
    }

    public override void ApplyEffect()
    {
        target.UpdateScore(scoreModifier);
        Destroy(this.gameObject);
    }
}
