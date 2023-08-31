using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeSelect : MonoBehaviour
{
    public Slider timeSlider;
    public TMP_Text selectedTimeText;
    public int selectedTime = 60;

    private void Start()
    {
        // Set initial value and update text
        timeSlider.value = selectedTime;
        UpdateSelectedTime();
    }

    public void OnSliderValueChanged()
    {
        UpdateSelectedTime();
    }

    public void UpdateSelectedTime()
    {
        selectedTime = Mathf.RoundToInt(timeSlider.value);
        selectedTimeText.text = "Temps choisi: " + selectedTime + " secondes";
    }
}
