using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the inventory HUD panels, highlighting the active slot and showing item icons.
/// </summary>
public class HUDInventory : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject[] panels; // References to the UI panels representing inventory slots

    [Header("Colors")]
    public Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Default semi-transparent gray
    public Color activeColor = new Color(1f, 0.5f, 0.5f, 1f);       // Highlight color for active slot

    private static InventoryManager inventory;
    private Image[] panelImages; // Cached Image components from the panels

    private void Start()
    {
        inventory = InventoryManager.Instance;

        // Cache Image components for better performance
        panelImages = new Image[panels.Length];
        for (int i = 0; i < panels.Length; i++)
        {
            panelImages[i] = panels[i].GetComponent<Image>();
            if (panelImages[i] == null)
            {
                Debug.LogWarning($"Panel at index {i} has no Image component.");
            }
        }

        UpdatePanelColors();
    }

    private void Update()
    {
        UpdatePanelColors();
    }

    /// <summary>
    /// Updates the UI panel colors and sprites based on the active slot and inventory content.
    /// </summary>
    private void UpdatePanelColors()
    {
        if (panels == null || inventory == null)
        {
            Debug.LogWarning("Panels or InventoryManager instance is not initialized.");
            return;
        }

        int activeSlot = inventory.getactiveItemSlot();

        for (int i = 0; i < panels.Length; i++)
        {
            if (panelImages[i] == null)
                continue;

            // Highlight active slot
            panelImages[i].color = (i == activeSlot) ? activeColor : defaultColor;

            // Set item image or disable if slot is empty
            if (i < inventory.items.Count && inventory.items[i] != null)
            {
                panelImages[i].sprite = inventory.items[i].itemImage;
                panelImages[i].enabled = true;
            }
            else
            {
                panelImages[i].sprite = null;
                panelImages[i].enabled = false;
            }
        }
    }
}
