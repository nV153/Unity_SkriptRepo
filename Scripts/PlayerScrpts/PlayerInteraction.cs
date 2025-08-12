using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player interactions with interactable objects within reach.
/// Casts a ray from the main camera forward to detect interactables,
/// manages outline highlighting and interaction UI, and triggers interaction on key press.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;                  // Maximum distance to interact with objects
    private Interactable currentInteractable;       // Currently focused interactable object

    public KeyCode interactKey = KeyCode.E;     // Public interaction key, default 'E'

    private void Update()
    {
        CheckInteraction();

        // Interact with the current interactable when the configured key is pressed
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    /// <summary>
    /// Raycasts forward from camera to detect interactable objects within reach.
    /// Manages enabling/disabling outlines and HUD interaction text.
    /// </summary>
    private void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                // Disable outline on previous interactable if switching targets
                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    /// <summary>
    /// Sets the given interactable as the current target,
    /// enables its outline and displays the interaction message on the HUD.
    /// </summary>
    /// <param name="newInteractable">The new interactable object.</param>
    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableInteractionText(currentInteractable.message);
    }

    /// <summary>
    /// Clears the current interactable target,
    /// disables its outline and clears the HUD interaction text.
    /// </summary>
    private void DisableCurrentInteractable()
    {
        HUDController.instance.DisableInteractionText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
