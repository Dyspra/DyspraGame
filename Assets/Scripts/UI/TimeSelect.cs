using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeSelect : MonoBehaviour
{
    public Slider timeSlider;
    public TMP_Text selectedTimeText;
    public int selectedTime = 5;
    public int stepAmount = 5;
    private int numberOfSteps = 0;

    private void Start()
    {
        // Set initial value and update text
        timeSlider.value = selectedTime;
        numberOfSteps = (int)timeSlider.maxValue / stepAmount;
        UpdateSelectedTime();
    }

    public void OnSliderValueChanged()
    {
        UpdateSelectedTime();
    }

    public void UpdateSelectedTime()
    {
        float range = (timeSlider.value / timeSlider.maxValue) * numberOfSteps;
        int ceil = Mathf.CeilToInt(range);
        timeSlider.value = ceil * stepAmount;
        selectedTime = ceil * stepAmount * 60;
        selectedTimeText.text = "Temps choisi: " + ceil * stepAmount + " minutes";
    }
}
