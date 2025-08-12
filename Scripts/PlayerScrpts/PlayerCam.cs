using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's camera rotation based on mouse input.
/// Handles mouse sensitivity and clamps vertical rotation to prevent flipping.
/// Also rotates the player orientation horizontally.
/// </summary>
public class PlayerCam : MonoBehaviour
{
    public float sensX = 100f;           // Mouse sensitivity for horizontal movement
    public float sensY = 100f;           // Mouse sensitivity for vertical movement

    public Transform Orientation;        // Reference to player orientation transform

    private float xRotation = 0f;        // Current vertical rotation (pitch)
    private float yRotation = 0f;        // Current horizontal rotation (yaw)

    void Start()
    {
        // Lock cursor to the game window and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input, scaled by sensitivity and deltaTime for smooth movement
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;  // <-- sensY for vertical

        yRotation += mouseX;
        xRotation -= mouseY;

        // Clamp vertical rotation to prevent flipping the camera upside down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera (this transform)
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Rotate the player orientation only on the y-axis (yaw)
        Orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
