using UnityEngine;
using System.Collections;

/// <summary>
/// Implements a projectile attack with an optional warning indicator before firing.
/// </summary>
public class ProjectileAttack : MonoBehaviour, IAttackPattern
{
    /// <summary>
    /// Prefab of the projectile to instantiate and launch.
    /// </summary>
    public GameObject projectilePrefab;

    /// <summary>
    /// Speed at which the projectile travels.
    /// </summary>
    public float projectileSpeed = 10f;

    /// <summary>
    /// Duration in seconds for the warning indicator before firing.
    /// </summary>
    public float warningDuration = 1.5f;

    /// <summary>
    /// Prefab of the warning indicator shown before firing.
    /// </summary>
    public GameObject warningPrefab;

    /// <summary>
    /// Maximum range of the attack (used by other systems or AI).
    /// </summary>
    public float Range => 50f;

    /// <summary>
    /// Coroutine that executes the projectile attack towards a specific target.
    /// Displays warning, spawns projectile, and sets its velocity.
    /// </summary>
    /// <param name="target">The transform of the target to attack.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("ProjectileAttack: No target provided.");
            yield break;
        }

        Vector3 targetPosition = target.position;
        Vector3 spawnPosition = transform.position;

        // Calculate normalized direction from spawner to target
        Vector3 direction = (targetPosition - spawnPosition).normalized;

        // Spawn position offset slightly forward from spawner to avoid self-collision
        Vector3 spawnPos = spawnPosition + direction * 1.5f;

        // 1. Show optional warning indicator before firing
        if (warningPrefab != null)
        {
            GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.LookRotation(direction));
            yield return new WaitForSeconds(warningDuration);
            Destroy(warning);
        }
        else
        {
            yield return new WaitForSeconds(warningDuration);
        }

        // 2. Instantiate the projectile facing the target direction
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));

        // 3. Prevent projectile from colliding with the spawner itself
        Collider ownCol = GetComponent<Collider>();
        Collider projCol = projectile.GetComponent<Collider>();
        if (ownCol != null && projCol != null)
        {
            Physics.IgnoreCollision(projCol, ownCol);
        }

        // 4. Assign velocity to the projectile using Rigidbody or fallback mover
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            // If no Rigidbody present, add a simple mover component for movement
            projectile.AddComponent<SimpleProjectileMover>().Init(direction, projectileSpeed);
        }
    }

    /// <summary>
    /// Parameterless ExecuteCoroutine required by IAttackPattern interface.
    /// Does nothing here as target is required.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ExecuteCoroutine()
    {
        yield return null;
    }
}

/// <summary>
/// Simple fallback projectile movement component if Rigidbody is not attached.
/// Moves the GameObject linearly in a given direction at a set speed.
/// </summary>
public class SimpleProjectileMover : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    /// <summary>
    /// Initializes the mover with a direction and speed.
    /// </summary>
    /// <param name="dir">Normalized direction vector.</param>
    /// <param name="spd">Speed in units per second.</param>
    public void Init(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    private void Update()
    {
        // Move the transform linearly every frame
        transform.position += direction * speed * Time.deltaTime;
    }
}
