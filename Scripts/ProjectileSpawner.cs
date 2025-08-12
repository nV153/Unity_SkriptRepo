using UnityEngine;

/// <summary>
/// Spawns a projectile prefab at a specified spawn point
/// and applies an initial forward velocity to it.
/// </summary>
public class ProjectileSpawner : MonoBehaviour
{
    /// <summary>
    /// The projectile prefab to spawn.
    /// Must have a Rigidbody component for movement.
    /// </summary>
    public GameObject projectilePrefab;

    /// <summary>
    /// The transform representing the position and rotation
    /// from which the projectile will be spawned.
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// The initial speed of the spawned projectile in units per second.
    /// </summary>
    public float projectileSpeed = 10f;

    /// <summary>
    /// Spawns the projectile at the spawn point and applies forward velocity.
    /// </summary>
    public void SpawnProjectile()
    {
        // Ensure both the projectile prefab and spawn point are set
        if (projectilePrefab != null && spawnPoint != null)
        {
            // Instantiate the projectile at the given position and rotation
            GameObject newProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

            // Get the Rigidbody to apply movement
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply velocity in the forward direction of the spawn point
                rb.linearVelocity = spawnPoint.forward * projectileSpeed;
            }
        }
        else
        {
            Debug.LogWarning("ProjectilePrefab or SpawnPoint not assigned!");
        }
    }
}
