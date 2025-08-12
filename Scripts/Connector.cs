using UnityEngine;

/// <summary>
/// Enum representing cardinal directions.
/// </summary>
public enum Direction
{
    North,
    East,
    South,
    West
}

/// <summary>
/// Connector component that holds a direction and visualizes it in the editor.
/// </summary>
public class Connector : MonoBehaviour
{
    /// <summary>
    /// Direction assigned to this connector.
    /// </summary>
    public Direction direction;

#if UNITY_EDITOR
    /// <summary>
    /// Draws a red ray and label in the Unity Editor scene view to indicate the direction.
    /// Only runs in the Editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2);

        // Draw direction label slightly in front of the object
        UnityEditor.Handles.Label(transform.position + transform.forward * 2, direction.ToString());
    }
#endif
}
