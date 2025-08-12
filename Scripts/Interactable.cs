using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents an interactable object that can be highlighted and respond to interactions.
/// </summary>
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// Reference to the Outline component used to highlight the object.
    /// </summary>
    private Outline outline;

    /// <summary>
    /// Optional message to display when interacting with the object.
    /// </summary>
    public string message;

    /// <summary>
    /// UnityEvent invoked when the object is interacted with.
    /// Can be configured in the Inspector to trigger various responses.
    /// </summary>
    public UnityEvent onInteraction;

    /// <summary>
    /// Initialize references and disable outline on start.
    /// </summary>
    private void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    /// <summary>
    /// Disables the outline highlighting.
    /// </summary>
    public void DisableOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }

    /// <summary>
    /// Enables the outline highlighting.
    /// </summary>
    public void EnableOutline()
    {
        if (outline != null)
            outline.enabled = true;
    }

    /// <summary>
    /// Invokes the interaction event.
    /// Called when the player interacts with the object.
    /// </summary>
    public void Interact()
    {
        onInteraction.Invoke();
    }
}
