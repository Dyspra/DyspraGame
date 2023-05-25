using UnityEngine;
using System.Collections;

public class RegularBall : IBall
{
    public int scoreModifier = 1;

    private Score target;

    [SerializeField] private Material matNormal;
    [SerializeField] private Material goldenMat;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Score>();
        Dyspra.AbstractObserver[] obsFounded = FindObjectsOfType<Dyspra.AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    public override void ApplyEffect()
    {
        if (target)
            target.UpdateScore(scoreModifier);
        NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_GET_BALLOON);
        StopAllCoroutines();
        Destroy(this.gameObject);
    }

    private IEnumerator GoldenTime(float duration)
    {
        this.GetComponent<MeshRenderer>().material = goldenMat;
        yield return new WaitForSeconds(duration);
        this.GetComponent<MeshRenderer>().material = matNormal;

    }

    public void StartGolden(float duration)
    {
        StartCoroutine(GoldenTime(duration));
    }
}
