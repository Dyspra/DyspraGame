using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : AJellyfishBehaviour
{
    public Material lighted_mat;
    public Material immun_mat;
    public MultipleAudioSource audioSource;
    public List<GameObject> lights;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Yellow") {
            ToAdd = 1;
            score.UpdateScore(ToAdd);
            //ChangeColor(lighted_mat);
            ChangeLight();
            audioSource.sound1.Play();
        }
        if (other.gameObject.tag == "Red" && isInvincible == false && isLightUp == true) {
            ToAdd = -1;
            score.UpdateScore(ToAdd);
            ChangeColor(base_mat);
            ChangeLight();
            audioSource.sound2.Play();
        }
        if (other.gameObject.tag == "Blue" && isLightUp == true) {
            isInvincible = true;
            ChangeColor(immun_mat);
            StartCoroutine(Timer());
            audioSource.sound3.Play();
        }
        if (other.gameObject.tag == "Green" && isLightUp == false && other.gameObject.GetComponent<JellyfishBehaviour>().isLightUp == true) {
            ToAdd = 1;
            score.UpdateScore(ToAdd);
            ChangeLight();
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
        foreach (GameObject light in lights)
        {
            light.GetComponent<Light>().color = new Color32(250, 0, 255, 255);
        }
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        ChangeColor(lighted_mat);
		foreach (GameObject light in lights)
		{
			light.GetComponent<Light>().color = new Color32(2, 63, 0, 255);
		}
	}

    protected void ChangeLight()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(!light.activeSelf);
        }
        isLightUp = lights[0].activeSelf;
    }
}