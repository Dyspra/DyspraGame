using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : StandaloneInputModule
{
    [SerializeField] private Image cursor;
    [SerializeField] private Image uiFill;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private GameObject endFingerPoint;
    public float duration;
    private float remainingDuration;
    PointerEventData pointerEventData;
    bool isHovering = false;
    Coroutine loading;
    Camera cam;

    void Start() {
        cam = Camera.main;
        //raycaster = GetComponentInParent<GraphicRaycaster>();
    }

    void Update()
    {
        if (endFingerPoint)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(endFingerPoint.transform.position);
            cursor.rectTransform.anchoredPosition = new Vector3(screenPos.x, screenPos.y, 0);
        }
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = cam.WorldToScreenPoint(pointer.position);
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        if (results.Count > 0 && isHovering == false)
        {
            Debug.DrawRay(pointerEventData.position, Vector3.forward, Color.red);
            Debug.Log("Trouvé " + results[0].gameObject.name);
            isHovering = true;
            Being(duration);
        } else if (results.Count == 0 && isHovering == true)
        {
            Debug.Log("Rien trouvé");
            StopCoroutine(loading);
            remainingDuration = duration;
            uiFill.fillAmount = 0;
            isHovering = false;
        } else
        {
            Debug.Log("result count = " + results.Count);
        }

    }

    private void Being(float second)
    {
        remainingDuration = second;
        loading = StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            uiFill.fillAmount = Mathf.InverseLerp(duration, 0, remainingDuration);
            remainingDuration -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        OnEnd();
    }

    private void OnEnd()
    {
        Input.simulateMouseWithTouches = true;
        Touch touch = new Touch();
        touch.position = cam.WorldToScreenPoint(pointer.position);
        var pointerData = GetTouchPointerEventData(touch, out bool b, out bool bb);
        ProcessTouchPress(pointerData, true, true);
        Debug.Log("End");
    }
}
