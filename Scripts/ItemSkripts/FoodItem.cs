using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodItem", menuName = "Inventory/Food")]
public class FoodItem : ItemData
{
    public int healthRestoreAmount;

    /// <summary>
    /// Overrides the base use behavior to restore health to the player.
    /// </summary>
    protected override void UseInternal()
    {
        if (hpSystem != null)
        {
            hpSystem.Heal(healthRestoreAmount);
        }
        else
        {
            Debug.LogWarning("HPSystem reference is null. Cannot heal.");
        }
    }
}
