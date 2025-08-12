using UnityEngine;

[CreateAssetMenu(fileName = "NewBowWeapon", menuName = "Inventory/Weapon/Bow")]
public class BowWeaponItem : ItemData
{
    [Header("Bow Weapon Settings")]
    public GameObject projectilePrefab;  // Prefab for the arrow/projectile
    public float projectileSpeed = 20f;  // Speed at which projectile is launched
    public int damage = 10;               // Damage dealt by the projectile

    /// <summary>
    /// Called when the bow is used.
    /// Spawns a projectile from the player's "HandObject" position with an upward offset.
    /// </summary>
    protected override void UseInternal()
    {
        // Find player GameObject by tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No player with tag 'Player' found.");
            return;
        }

        // Find the fire point transform on the player where projectile will spawn
        Transform firePoint = player.transform.Find("HandObject");
        if (firePoint == null)
        {
            Debug.LogWarning("No child named 'HandObject' found on player.");
            return;
        }

        // Offset spawn position by 2 units upward from the fire point
        Vector3 spawnPos = firePoint.position + Vector3.up * 2.0f;

        // Instantiate the projectile prefab at spawnPos with the same rotation as the firePoint
        if (projectilePrefab != null)
        {
            GameObject proj = Instantiate(projectilePrefab, spawnPos, firePoint.rotation);

            // Set projectile damage if Projectile component exists
            Projectile projScript = proj.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.damage = damage;
            }

            // Set projectile velocity if Rigidbody exists
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }

            // Play use sound (if any)
            PlayUseSound();
        }
        else
        {
            Debug.LogWarning("Projectile prefab not assigned.");
        }
    }
}
