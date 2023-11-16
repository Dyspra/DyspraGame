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
    private GameObject endFingerPoint;
    private GraphicRaycaster raycaster;
    public float duration;
    private float remainingDuration;
    PointerEventData pointerEventData;
    bool isHovering = false;
    Coroutine loading;
    Camera cam;
    private Vector2 canvasResolution;

    void Start() {
        cam = Camera.main;
        //StartCoroutine(LastCanvas());
        SearchLastCanvas();
        endFingerPoint = GameObject.FindWithTag("CursorFinger");
        canvasResolution = GetComponentInParent<CanvasScaler>().referenceResolution;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
			SearchLastCanvas();
		}
        if (raycaster != null)
        {
            if (endFingerPoint)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(endFingerPoint.transform.position);
                Debug.Log(endFingerPoint.transform.position);
                cursor.rectTransform.anchoredPosition = new Vector3(screenPos.x / 1920 * canvasResolution.x, screenPos.y / 1080 * canvasResolution.y, 0);
            }
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = cam.WorldToScreenPoint(pointer.position);
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);
            if (results.Count > 0 && isHovering == false)
            {
                Debug.DrawRay(pointerEventData.position, Vector3.forward, Color.red);
                //Debug.Log("Trouvé " + results[0].gameObject.name);
                isHovering = true;
                Being(duration);
            } else if (results.Count == 0 && isHovering == true)
            {
                //Debug.Log("Rien trouvé");
                StopCoroutine(loading);
                remainingDuration = duration;
                uiFill.fillAmount = 0;
                isHovering = false;
            } else
            {
                //Debug.Log("result count = " + results.Count);
            }
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
        if (this.isActiveAndEnabled) 
        {
            SearchLastCanvas();
        }
        remainingDuration = duration;
		uiFill.fillAmount = 0;
		isHovering = false;
		//Debug.Log("End");
    }

    private bool IsLastSibling()
    {
        return this.transform.GetSiblingIndex() == this.transform.parent.childCount - 1;
    }

    private IEnumerator LastCanvas()
    {
        yield return new WaitForSeconds(0.5f);
        GraphicRaycaster[] graphicRaycaster = FindObjectsOfType<GraphicRaycaster>();
        int maxPriority = -1;
        foreach(GraphicRaycaster raycast in graphicRaycaster)
        {
            int order = raycast.GetComponent<Canvas>().sortingOrder;

            Debug.Log("RAYCAST = " + raycast.gameObject.name);
			if ( order > maxPriority)
            {
                maxPriority = order;
                raycaster = raycast;
            }
        }
        Debug.Log("FINAL RAYCASTER = " + raycaster.gameObject.name);
    }

    public void SearchLastCanvas()
    {
        StartCoroutine(LastCanvas());
    }
}
