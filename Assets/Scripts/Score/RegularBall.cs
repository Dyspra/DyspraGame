using UnityEngine;
using System.Collections;

public class RegularBall : IBall
{
    public int scoreModifier = 1;

    private Score target;
    private AudioSource audioSource;

    [SerializeField] private Material matNormal;
    [SerializeField] private Material goldenMat;

    bool hasScored = false;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Score>();
        audioSource = GetComponent<AudioSource>();
        Dyspra.AbstractObserver[] obsFounded = FindObjectsOfType<Dyspra.AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    public override void ApplyEffect()
    {
        if (target && hasScored == false)
            target.UpdateScore(scoreModifier);
        if (hasScored == false)
        {
            hasScored = true;
            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_GET_BALLOON);
        }
        StopAllCoroutines();
        AudioSource.PlayClipAtPoint(audioSource.clip, this.transform.position);
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
