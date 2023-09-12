using UnityEngine;
using UnityEngine.UI;

public class ProgressBarWithText : MonoBehaviour
{
    public Slider slider;
    public Text progressText;

    private void Start()
    {
        UpdateProgressText();
    }

    private void Update()
    {
        UpdateProgressText();
    }

    private void UpdateProgressText()
    {
        float progress = slider.value * 100f; // Convertir la valeur du Slider en pourcentage
        progressText.text = progress.ToString("F0") + "%"; // Afficher le pourcentage avec 0 décimales
    }
}

