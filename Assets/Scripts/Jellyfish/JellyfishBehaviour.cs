using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : AJellyfishBehaviour
{
    public Material lighted_mat;
    public Material immun_mat;
    public MultipleAudioSource audioSource;
    public List<GameObject> lights;
    public float runAwayDuration = 3f;
    private bool isLightable = true;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Yellow" && isLightUp == false && isLightable == true) {
            isLightUp = true;
            ToAdd = 1;
            moveSpeed = 5f;
            score.UpdateJellyfishLit(ToAdd);
            ChangeColor(lighted_mat);
            audioSource.sound1.Play();
        }
        if (other.gameObject.tag == "Red" && isInvincible == false && isLightUp == true) {
			isLightUp = false;
			ToAdd = -1;
            moveSpeed = 1f;
            score.UpdateJellyfishLit(ToAdd);
            ChangeColor(base_mat);
            audioSource.sound2.Play();
            StartCoroutine(RunAway());
        }
        if ((other.gameObject.tag == "Blue" && isLightUp == true) || (other.gameObject.tag == "Green" && other.gameObject.GetComponent<JellyfishBehaviour>().isInvincible == true)) {
            isInvincible = true;
            ChangeColor(immun_mat);
            StartCoroutine(Timer());
            audioSource.sound3.Play();
        }
        if (other.gameObject.tag == "Green" && isLightUp == false && other.gameObject.GetComponent<JellyfishBehaviour>().isLightUp == true && isLightable == true) {
			isLightUp = true;
			ToAdd = 1;
            moveSpeed = 5f;
            score.UpdateJellyfishLit(ToAdd);
			ChangeColor(lighted_mat);
			audioSource.sound1.Play();
		}
    }
    protected void ChangeColor(Material mat)
    {
        foreach(Renderer r in _renderer) {
            r.material = mat;
        }
    }
    protected IEnumerator Timer()
    {
        /*foreach (GameObject light in lights)
        {
            light.GetComponent<Light>().color = new Color32(250, 0, 255, 255);
        }*/
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        ChangeColor(lighted_mat);
		/*foreach (GameObject light in lights)
		{
			light.GetComponent<Light>().color = new Color32(2, 63, 0, 255);
		}*/
	}

    protected void ChangeLight()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(!light.activeSelf);
        }
        isLightUp = lights[0].activeSelf;
    }

    IEnumerator RunAway()
    {
        moveSpeed = 10f;
        isLightable = false;
        yield return new WaitForSeconds(runAwayDuration);
        moveSpeed = 1f;
        isLightable = true;
    }
}