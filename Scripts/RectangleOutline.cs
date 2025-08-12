using UnityEngine;

/// <summary>
/// Draws a rectangular outline in the scene using a LineRenderer.
/// Used to visually indicate areas where attacks will occur.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RectangleOutline : MonoBehaviour
{
    /// <summary>
    /// Width of the rectangle in world units.
    /// </summary>
    public float width = 5f;

    /// <summary>
    /// Height of the rectangle in world units.
    /// </summary>
    public float height = 3f;

    /// <summary>
    /// Color of the rectangle outline (default: semi-transparent red).
    /// </summary>
    public Color color = new Color(1, 0, 0, 0.7f);

    /// <summary>
    /// Optional material for the LineRenderer.
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// Reference to the LineRenderer component.
    /// </summary>
    private LineRenderer lr;

    /// <summary>
    /// Initializes the LineRenderer and draws the rectangle.
    /// </summary>
    private void Start()
    {
        // Get or ensure a LineRenderer component exists
        lr = GetComponent<LineRenderer>();

        // Configure the line properties
        lr.positionCount = 5; // 4 corners + repeat first point to close shape
        lr.loop = true;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = lineMaterial;
        lr.startColor = color;
        lr.endColor = color;

        // Use the object's position as the rectangle's center
        Vector3 center = transform.position;

        // Calculate the rectangle's corner positions
        Vector3[] points = new Vector3[5];
        points[0] = center + new Vector3(-width / 2, 0, -height / 2);
        points[1] = center + new Vector3(-width / 2, 0, height / 2);
        points[2] = center + new Vector3(width / 2, 0, height / 2);
        points[3] = center + new Vector3(width / 2, 0, -height / 2);
        points[4] = points[0]; // Close the loop

        // Apply positions to the LineRenderer
        lr.SetPositions(points);
    }
}
