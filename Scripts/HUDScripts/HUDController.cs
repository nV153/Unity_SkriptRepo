using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Singleton class to manage HUD elements like interaction prompts.
/// </summary>
public class HUDController : MonoBehaviour
{
    public static HUDController instance;

    [SerializeField] private TMP_Text interactionText;

    private void Awake()
    {
        // Assign singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Shows interaction text with a standard interaction key hint (E).
    /// </summary>
    /// <param name="text">The base interaction message.</param>
    public void EnableInteractionText(string text)
    {
        if (interactionText != null)
        {
            interactionText.text = $"{text} (E)";
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Interaction Text UI component is not assigned.");
        }
    }

    /// <summary>
    /// Hides the interaction text.
    /// </summary>
    public void DisableInteractionText()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
