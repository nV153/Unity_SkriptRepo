using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;  // Reference to the ScriptableObject holding item data

    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        if (outline != null)
        {
            DisableOutline();
        }
    }

    /// <summary>
    /// Disables the outline effect (e.g., when not focused or selected)
    /// </summary>
    public void DisableOutline()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    /// <summary>
    /// Enables the outline effect (e.g., when player looks at the item)
    /// </summary>
    public void EnableOutline()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    /// <summary>
    /// Called when the player interacts with the item (e.g., presses interact key)
    /// </summary>
    public void Interact()
    {
        Pickup();
    }

    /// <summary>
    /// Adds the item to the player's inventory if there is space, then destroys the item in the scene.
    /// </summary>
    public void Pickup()
    {
        // Assuming max inventory size is 6
        if (InventoryManager.Instance.getcount() < 6)
        {
            InventoryManager.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full! Cannot pick up " + itemData.itemName);
        }
    }
}
