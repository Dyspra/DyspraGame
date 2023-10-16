using UnityEngine;

public class Axes : MonoBehaviour
{
    public float xAxisLength = 10f;
    public float yAxisLength = 10f;
    public float lineWidth = 0.1f;
    public Color axisColor = Color.black;

    void Start()
    {
        CreateAxis(new Vector3(-xAxisLength / 2, 0f, 0f), new Vector3(xAxisLength / 2, 0f, 0f));

        CreateAxis(new Vector3(0f, -yAxisLength / 2, 0f), new Vector3(0f, yAxisLength / 2, 0f));
    }

    void CreateAxis(Vector3 start, Vector3 end)
    {
        GameObject axisObject = new GameObject("Axis");
        axisObject.transform.SetParent(transform);

        LineRenderer lineRenderer = axisObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material.color = axisColor;
    }
}
