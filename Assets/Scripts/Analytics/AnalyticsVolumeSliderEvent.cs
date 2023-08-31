using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnalyticsVolumeSliderEvent : MonoBehaviour, IPointerUpHandler
{
    public string volumeType;

    public void OnPointerUp(PointerEventData eventData)
    {
        float volume = GetComponent<UnityEngine.UI.Slider>().value;
        AnalyticsManager.Instance.LogS_ChangeVolume(volumeType, volume);
    }
}
