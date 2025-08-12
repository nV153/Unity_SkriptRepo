using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns a warning zone and then an attack zone at a random direction and distance from the origin or target.
/// </summary>
public class DirectionalAttack : MonoBehaviour, IAttackPattern
{
    [Tooltip("Prefab for the attack zone.")]
    public GameObject zonePrefab;

    [Tooltip("Prefab for the warning zone.")]
    public GameObject warningZonePrefab;

    [Tooltip("Duration (in seconds) the warning zone is visible before the attack.")]
    public float warningDuration = 2f;

    [Tooltip("Minimum distance from the origin/target to spawn the attack.")]
    public float minDistance = 5f;

    [Tooltip("Maximum distance from the origin/target to spawn the attack.")]
    public float maxDistance = 20f;

    [Tooltip("Angle (in degrees) for the possible attack directions (0-360).")]
    public float attackangle = 360f;

    /// <summary>
    /// The maximum range of the attack.
    /// </summary>
    public float Range => maxDistance;

    /// <summary>
    /// Executes the attack pattern as a coroutine, targeting a specific Transform.
    /// </summary>
    /// <param name="target">The target Transform. If null, uses this object's position.</param>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        Vector3 origin = transform.position;

        if (target != null)
        {
            origin = target.position;  // Or calculate from enemy position to target, if needed
        }

        Vector3 randomDirection = GetRandomDirectionXZ();
        float randomDistance = Random.Range(minDistance, maxDistance);

        Vector3 spawnPos = origin + randomDirection * randomDistance;

        yield return SpawnWarningAndAttack(spawnPos);
    }

    /// <summary>
    /// Executes the attack pattern as a coroutine without a specific target.
    /// </summary>
    public IEnumerator ExecuteCoroutine()
    {
        yield return ExecuteCoroutine(null);
    }

    /// <summary>
    /// Returns a random normalized direction vector on the XZ plane within the specified attack angle.
    /// </summary>
    private Vector3 GetRandomDirectionXZ()
    {
        float angle = Random.Range(0f, attackangle);
        float rad = angle * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)).normalized;
    }

    /// <summary>
    /// Spawns a warning zone at the given position, waits for the warning duration, then spawns the attack zone.
    /// </summary>
    /// <param name="position">The position to spawn the zones.</param>
    private IEnumerator SpawnWarningAndAttack(Vector3 position)
    {
        RaycastHit hit;
        Vector3 rayStart = position + Vector3.up * 5f;
        Vector3 rayDirection = Vector3.down;
        float rayDistance = 10f;

        // Cast a ray downward to find the ground
        if (Physics.Raycast(rayStart, rayDirection, out hit, rayDistance))
        {
            Vector3 groundPosition = hit.point;

            GameObject warning = Instantiate(warningZonePrefab, groundPosition, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);
            Destroy(warning);

            Instantiate(zonePrefab, groundPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("DirectionalAttack could not find ground! Attack aborted.");
        }
    }
}
