using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player interaction with items within reach.
/// Casts a ray from the camera forward to detect items,
/// highlights the currently focused item, and allows interaction on key press.
/// </summary>
public class PlayerItemInteraction : MonoBehaviour
{
    public float playerReach = 3f; // Maximum distance to interact with items
    private Item currentItem;      // Currently focused item

    private void Update()
    {
        CheckInteraction();

        // Interact with the current item when player presses 'F'
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            currentItem.Interact();
        }
    }

    /// <summary>
    /// Casts a ray from the camera to detect items within reach.
    /// Manages enabling/disabling outlines and interaction UI.
    /// </summary>
    private void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.CompareTag("Item"))
            {
                Item newItem = hit.collider.GetComponent<Item>();

                // If the current item is different from the newly detected one, disable outline on old item
                if (currentItem && newItem != currentItem)
                {
                    currentItem.DisableOutline();
                }

                if (newItem.enabled)
                {
                    SetNewCurrentItem(newItem);
                }
                else
                {
                    DisableCurrentItem();
                }
            }
            else
            {
                DisableCurrentItem();
            }
        }
        else
        {
            DisableCurrentItem();
        }
    }

    /// <summary>
    /// Sets the given item as the current focused item and enables its outline and interaction UI.
    /// </summary>
    /// <param name="newItem">The new item to focus on.</param>
    private void SetNewCurrentItem(Item newItem)
    {
        currentItem = newItem;
        currentItem.EnableOutline();
        HUDPickUp.instance.EnableInteractionText(currentItem.itemData.itemName);
    }

    /// <summary>
    /// Clears the current focused item, disables outline and interaction UI.
    /// </summary>
    private void DisableCurrentItem()
    {
        HUDPickUp.instance.DisableInteractionText();
        if (currentItem)
        {
            currentItem.DisableOutline();
            currentItem = null;
        }
    }
}
