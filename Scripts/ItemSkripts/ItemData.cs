using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int itemID = -1;                // Unique ID for the item
    public string itemName = "test";       // Display name of the item
    public bool isConsumable = false;      // Indicates if the item is consumable
    public Sprite itemImage;               // UI sprite for the item icon
    public GameObject prefab;              // Reference to the prefab representing the item in the world
    protected HPSystem hpSystem;           // Reference to HP system, set when used
    public string useSoundName = "";       // Name of the sound to play when the item is used

    /// <summary>
    /// Internal method called when the item is used.
    /// Override this in subclasses to define custom use behavior.
    /// </summary>
    protected virtual void UseInternal()
    {
        Debug.Log("Item used: " + itemName);
    }

    /// <summary>
    /// Public method to use the item, passing the HP system (or similar) that might be affected.
    /// </summary>
    /// <param name="hpSystemInstance">Reference to the HP system or similar context</param>
    public void Use(HPSystem hpSystemInstance)
    {
        hpSystem = hpSystemInstance;
        PlayUseSound();
        UseInternal();
    }

    /// <summary>
    /// Plays the use sound if a valid sound name is set and AudioManager is available.
    /// </summary>
    protected void PlayUseSound()
    {
        if (!string.IsNullOrEmpty(useSoundName))
        {
            AudioManager instance = AudioManager.instance;
            if (instance != null)
            {
                instance.Play(useSoundName);
            }
            else
            {
                Debug.LogWarning("AudioManager instance not found.");
            }
        }
    }
}
