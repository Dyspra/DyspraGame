using UnityEngine;
using UnityEngine.UI;

public class LightChange : MonoBehaviour
{
    public Slider intensitySlider;
    public Light controlledLight;
    public Text intensityText;

    private float minIntensity = 0.1f;
    private float maxIntensity = 3.0f;

    private void Start()
    {
        intensitySlider.minValue = minIntensity;
        intensitySlider.maxValue = maxIntensity;
        intensitySlider.value = controlledLight.intensity;
    }

    private void Update()
    {
        float newIntensity = intensitySlider.value;
        controlledLight.intensity = newIntensity;

        intensityText.text = "Intensité : " + newIntensity.ToString("F1");
    }
}