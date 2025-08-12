using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton class managing the player's inventory of items,
/// allowing item selection, usage, dropping, and displaying equipped items.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of InventoryManager.
    /// </summary>
    public static InventoryManager Instance;

    private int count = 0; // Number of items currently in inventory
    private ItemData activeItem; // Currently equipped item
    private int activeItemSlot = 0; // Index of the currently active item slot

    /// <summary>
    /// Reference to the HPSystem for applying item effects.
    /// </summary>
    public HPSystem hpSystem;

    /// <summary>
    /// Fixed-size array of item slots (size 6).
    /// </summary>
    public ItemData[] items = new ItemData[6];

    /// <summary>
    /// Transform representing the player's hand where equipped items are shown.
    /// Should be assigned in the Inspector.
    /// </summary>
    public Transform handSlot;

    /// <summary>
    /// Currently visible equipped item's GameObject instance.
    /// </summary>
    private GameObject currentEquippedGO;

    /// <summary>
    /// Force applied to items when thrown/dropped.
    /// </summary>
    public float throwStrength = 3f;

    /// <summary>
    /// Key to drop the active item.
    /// </summary>
    public KeyCode dropKey;

    /// <summary>
    /// Key to use the active item.
    /// </summary>
    public KeyCode useKey;

    private void Awake()
    {
        // Initialize singleton instance or destroy duplicates
        if (Instance == null)
        {
            Instance = this;
            activeItem = null;
            // Optional: preserve inventory on scene load
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Handle scrolling to cycle through active item slots
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            activeItemSlot = (activeItemSlot != 5) ? activeItemSlot + 1 : 0;
            activeItem = items[activeItemSlot];
            ShowEquippedItem(activeItem);
        }
        else if (scroll < 0f)
        {
            activeItemSlot = (activeItemSlot != 0) ? activeItemSlot - 1 : 5;
            activeItem = items[activeItemSlot];
            ShowEquippedItem(activeItem);
        }

        // Drop active item when drop key is pressed
        if (Input.GetKeyDown(dropKey))
        {
            if (activeItem != null && activeItem.prefab != null)
            {
                DropActiveItem();
            }
        }

        // Use active item when use key is pressed
        if (Input.GetKeyDown(useKey))
        {
            if (activeItem != null)
            {
                activeItem.Use(hpSystem);
                if (activeItem.isConsumable)
                {
                    RemoveItem(activeItemSlot);
                }
            }
        }
    }

    /// <summary>
    /// Drops the currently active item into the world with a throwing force.
    /// Removes the item from the inventory.
    /// </summary>
    private void DropActiveItem()
    {
        Transform ori = transform.Find("Orientation");
        if (ori != null)
        {
            Vector3 spawnPosition = ori.position + ori.forward * 2.0f + Vector3.up;
            GameObject droppedItem = Instantiate(activeItem.prefab, spawnPosition, Quaternion.identity);

            // Apply throwing force to dropped item
            Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Throw direction slightly upwards
                Vector3 throwDirection = ori.forward + Vector3.up * 0.08f;
                Vector3 baseForce = throwDirection.normalized * throwStrength;

                // Add player's momentum for realistic throw
                Rigidbody playerRb = GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    baseForce += playerRb.linearVelocity * 0.5f; // Weight of player momentum
                }

                rb.AddForce(baseForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
            }

            activeItem = null;
            items[activeItemSlot] = null;
            count--;
            ShowEquippedItem(activeItem);
        }
    }

    /// <summary>
    /// Adds an item to the first free inventory slot.
    /// Updates the equipped item display.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public void AddItem(ItemData item)
    {
        items[FindFirstFreeIndex(items)] = item;
        count++;
        activeItem = items[activeItemSlot];
        ShowEquippedItem(activeItem);
    }

    /// <summary>
    /// Removes an item from the specified inventory slot.
    /// Updates the equipped item display.
    /// </summary>
    /// <param name="itemSlot">Index of the slot to remove the item from.</param>
    public void RemoveItem(int itemSlot)
    {
        items[itemSlot] = null;
        count--;
        activeItem = items[activeItemSlot];
        ShowEquippedItem(activeItem);
    }

    /// <summary>
    /// Debug function to print all items currently in the inventory.
    /// </summary>
    public void PrintInventory()
    {
        Debug.Log("Inventory:");
        foreach (ItemData item in items)
        {
            if (item != null)
            {
                Debug.Log("ItemID: " + item.itemID + " Name: " + item.itemName);
                Debug.Log("ActiveItem: " + (activeItem != null ? activeItem.itemName : "None"));
            }
        }
    }

    /// <summary>
    /// Gets the current number of items in the inventory.
    /// </summary>
    /// <returns>Number of items.</returns>
    public int GetCount()
    {
        return count;
    }

    /// <summary>
    /// Gets the index of the currently active item slot.
    /// </summary>
    /// <returns>Active item slot index.</returns>
    public int GetActiveItemSlot()
    {
        return activeItemSlot;
    }

    /// <summary>
    /// Gets the array of inventory items.
    /// </summary>
    /// <returns>Array of ItemData objects.</returns>
    public ItemData[] GetItems()
    {
        return items;
    }

    /// <summary>
    /// Finds the first free (null) slot index in the given array.
    /// Returns -1 if no free slot is found.
    /// </summary>
    /// <param name="array">Array of ItemData.</param>
    /// <returns>Index of first free slot or -1 if none found.</returns>
    public static int FindFirstFreeIndex(ItemData[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == null)
            {
                return i;
            }
        }
        return -1; // No free slot available
    }

    /// <summary>
    /// Updates the equipped item displayed in the player's hand.
    /// Destroys the previous equipped model and spawns the new one.
    /// Disables physics on the equipped model.
    /// </summary>
    /// <param name="itemData">ItemData to display as equipped.</param>
    private void ShowEquippedItem(ItemData itemData)
    {
        // Remove old equipped model
        if (currentEquippedGO != null)
        {
            Destroy(currentEquippedGO);
        }

        // Spawn new equipped model if item data is valid
        if (itemData != null && itemData.prefab != null)
        {
            currentEquippedGO = Instantiate(itemData.prefab, handSlot);
            currentEquippedGO.transform.localPosition = Vector3.zero;
            currentEquippedGO.transform.localRotation = Quaternion.identity;

            // Disable physics components on equipped item
            Rigidbody rb = currentEquippedGO.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Collider col = currentEquippedGO.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}
