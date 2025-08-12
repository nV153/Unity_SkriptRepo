using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Singleton HUD controller for showing item pickup interaction prompts.
/// </summary>
public class HUDPickUp : MonoBehaviour
{
    public static HUDPickUp instance;

    [SerializeField]
    private TMP_Text ItemPickText;

    private void Awake()
    {
        // Ensure singleton instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Enables the interaction text on screen with the item name and key prompt.
    /// </summary>
    /// <param name="text">Name of the item to pick up</param>
    public void EnableInteractionText(string text)
    {
        ItemPickText.text = $"Pick up Item: {text} (F)";
        ItemPickText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the interaction text.
    /// </summary>
    public void DisableInteractionText()
    {
        ItemPickText.gameObject.SetActive(false);
    }
}
