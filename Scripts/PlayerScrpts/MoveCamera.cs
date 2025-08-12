using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves this camera's position to match the target transform's position every frame.
/// Useful for following or syncing camera position with another object.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; // Target position to follow

    void Update()
    {
        // Update this object's position to the target's position
        transform.position = cameraPosition.position;
    }
}
