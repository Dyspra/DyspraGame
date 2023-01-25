using UnityEngine;

public class DottedOutline : MonoBehaviour
{
    public Material outlineMaterial;
    public float dotSpacing = 0.1f;
    public float dotSize = 0.05f;
    public int sortOrder = 0;

    private LineRenderer lineRenderer;

    private void Start()
    {
        // Create a new LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material and width of the dots
        lineRenderer.material = outlineMaterial;
        lineRenderer.startWidth = dotSize;
        lineRenderer.endWidth = dotSize;
        lineRenderer.textureMode = LineTextureMode.Tile;

        // Set the sort order to make sure the outline is rendered on top
        lineRenderer.sortingOrder = sortOrder;

        // Get the mesh of the object
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Get the vertices of the mesh
        Vector3[] vertices = mesh.vertices;

        // Calculate the positions of the dots
        Vector3[] dotPositions = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            dotPositions[i] = transform.TransformPoint(vertices[i]);
        }

        // Set the positions of the dots in the LineRenderer component
        lineRenderer.positionCount = dotPositions.Length;
        lineRenderer.SetPositions(dotPositions);

        // Set the spacing between the dots
        lineRenderer.numCornerVertices = Mathf.RoundToInt(dotSpacing / dotSize);
        lineRenderer.numCapVertices = Mathf.RoundToInt(dotSpacing / dotSize);
    }
}
