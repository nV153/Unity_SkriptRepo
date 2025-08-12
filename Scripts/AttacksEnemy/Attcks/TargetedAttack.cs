using UnityEngine;
using System.Collections;

/// <summary>
/// Attack pattern that targets a detected enemy or specified transform,
/// spawns a warning zone before the actual attack zone appears at the target's position.
/// Optionally snaps the attack zone to the ground below the target.
/// </summary>
public class TargetedAttack : MonoBehaviour, IAttackPattern
{
    /// <summary>
    /// Prefab for the actual attack zone spawned after warning.
    /// </summary>
    public GameObject zonePrefab;

    /// <summary>
    /// Prefab for the warning zone displayed before the attack.
    /// </summary>
    public GameObject warningZonePrefab;

    /// <summary>
    /// Duration in seconds to display the warning zone before the attack.
    /// </summary>
    public float warningDuration = 2f;

    /// <summary>
    /// If true, attempts to snap the attack zone to the ground below the target position.
    /// </summary>
    public bool snapToGround = true;

    [Header("Attack Settings")]

    [SerializeField]
    private float range = 50f; // Range of the attack, visible in Inspector

    /// <summary>
    /// Interface implementation to provide attack range.
    /// </summary>
    public float Range => range;

    /// <summary>
    /// Detection method used to find a target if none is provided.
    /// </summary>
    public IDetectionMethod detectionMethod;

    private void Awake()
    {
        detectionMethod = GetComponent<IDetectionMethod>();
        if (detectionMethod == null)
            Debug.LogWarning("No IDetectionMethod component found on this GameObject.");
    }

    /// <summary>
    /// Executes the attack coroutine on a specific target transform.
    /// If the target is null, tries to detect one automatically.
    /// </summary>
    /// <param name="target">Target transform to attack</param>
    /// <returns>Coroutine IEnumerator</returns>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        // If no target specified, try to detect one
        if (target == null)
        {
            if (detectionMethod == null)
            {
                Debug.LogWarning("No IDetectionMethod available for target detection.");
                yield break;
            }

            target = detectionMethod.DetectTarget(transform);

            if (target == null)
            {
                Debug.Log("No target detected for targeted attack.");
                yield break;
            }
        }

        // Run the warning display and actual attack sequence
        yield return SpawnWarningAndAttack(target);
    }

    /// <summary>
    /// Optional parameterless coroutine execution method.
    /// Not used in this pattern.
    /// </summary>
    /// <returns>Empty IEnumerator</returns>
    public IEnumerator ExecuteCoroutine()
    {
        yield break;
    }

    /// <summary>
    /// Handles spawning the warning zone, waiting, then spawning the attack zone at the target.
    /// Optionally snaps the attack zone position to the ground under the target.
    /// </summary>
    /// <param name="target">Target transform</param>
    /// <returns>Coroutine IEnumerator</returns>
    private IEnumerator SpawnWarningAndAttack(Transform target)
    {
        Vector3 attackPosition = target.position;

        if (snapToGround)
        {
            // Cast a ray downwards from above the target to find the ground position
            Vector3 rayStart = attackPosition + Vector3.up * 5f;
            Vector3 rayDirection = Vector3.down;
            float rayDistance = 10f;

            RaycastHit[] hits = Physics.RaycastAll(rayStart, rayDirection, rayDistance);

            bool groundFound = false;

            foreach (var hit in hits)
            {
                // Ignore hits on the target or its children (avoid snapping to target's colliders)
                if (hit.collider.transform == target || hit.collider.transform.IsChildOf(target))
                    continue;

                attackPosition = hit.point;
                groundFound = true;
                break;
            }

            if (!groundFound)
            {
                Debug.LogWarning("TargetedAttack could not find ground below target. Attack aborted.");
                yield break;
            }
        }

        // Spawn warning zone at the attack position
        GameObject warning = Instantiate(warningZonePrefab, attackPosition, Quaternion.identity);

        // Wait for the warning duration
        yield return new WaitForSeconds(warningDuration);

        // Destroy warning zone
        Destroy(warning);

        // Spawn the actual attack zone
        Instantiate(zonePrefab, attackPosition, Quaternion.identity);
    }

    /// <summary>
    /// Draws a wire sphere in the editor to visualize the detection radius if using Vision360Detection.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (detectionMethod is Vision360Detection d)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, d.scanRadius);
        }
    }
}
