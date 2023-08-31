using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBehaviour : AJellyfishBehaviour
{
    public Material lighted_mat;
    public Material immun_mat;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Yellow") {
            ToAdd = 1;
            score.UpdateScore(ToAdd);
            ChangeColor(lighted_mat);
            isLightUp = true;
        }
        if (other.gameObject.tag == "Red" && isInvincible == false && isLightUp == true) {
            ToAdd = -1;
            score.UpdateScore(ToAdd);
            ChangeColor(base_mat);
            isLightUp = false;
        }
        if (other.gameObject.tag == "Blue" && isLightUp == true) {
            isInvincible = true;
            ChangeColor(immun_mat);
            StartCoroutine(Timer());
        }
        if (other.gameObject.tag == "Green" && isLightUp == false && other.gameObject.GetComponent<JellyfishBehaviour>().isLightUp == true) {
            ToAdd = 1;
            score.UpdateScore(ToAdd);
            isLightUp = true;
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