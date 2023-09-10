using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : AJellyfishBehaviour
{
    public Material lighted_mat;
    public Material immun_mat;
    public MultipleAudioSource audioSource;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Yellow") {
            ToAdd = 1;
            score.UpdateScore(ToAdd);
            ChangeColor(lighted_mat);
            isLightUp = true;
            audioSource.sound1.Play();
        }
        if (other.gameObject.tag == "Red" && isInvincible == false && isLightUp == true) {
            ToAdd = -1;
            score.UpdateScore(ToAdd);
            ChangeColor(base_mat);
            isLightUp = false;
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
            isLightUp = true;
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
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        ChangeColor(lighted_mat);
    }
}