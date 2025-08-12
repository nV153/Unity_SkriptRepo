using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Implements a slash melee attack with a preparation phase, warning zone, and damage dealing.
/// </summary>
public class SlashAttack : MonoBehaviour, IAttackPattern
{
    [Header("Attack Settings")]

    /// <summary>
    /// Amount of damage dealt by the slash attack.
    /// </summary>
    public float damage = 25f;

    /// <summary>
    /// Force applied to knock back targets hit.
    /// </summary>
    public float knockbackForce = 15f;

    /// <summary>
    /// Range of the attack (also determines warning zone and damage zone size).
    /// </summary>
    public float attackRange = 3f;

    /// <summary>
    /// Duration in seconds that the damage zone remains active.
    /// </summary>
    public float attackDuration = 0.5f;

    /// <summary>
    /// Preparation time before attack damage is applied (enemy stands still).
    /// </summary>
    public float preparationTime = 1f;

    [Header("Warning Zone")]

    /// <summary>
    /// Prefab for the warning zone shown during preparation.
    /// </summary>
    public GameObject warningZonePrefab;

    /// <summary>
    /// Reference to the currently spawned warning zone instance.
    /// </summary>
    private GameObject currentWarningZone;

    [Header("Sound (Optional)")]

    /// <summary>
    /// AudioSource component to play sounds.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Sound played during the preparation phase.
    /// </summary>
    public AudioClip prepSound;

    /// <summary>
    /// Sound played when the attack is executed.
    /// </summary>
    public AudioClip attackSound;

    /// <summary>
    /// Implements the attack range for IAttackPattern interface.
    /// </summary>
    public float Range => attackRange;

    /// <summary>
    /// Keeps track of targets already damaged during the attack to avoid multiple hits.
    /// </summary>
    private HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

    /// <summary>
    /// Executes the slash attack coroutine targeting a specific transform.
    /// Shows warning zone, plays sounds, and applies damage with knockback.
    /// </summary>
    /// <param name="target">Target transform (can be null)</param>
    /// <returns>Coroutine IEnumerator</returns>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        if (target == null)
            yield break;

        // Calculate warning zone position slightly in front of the attacker
        Vector3 warnPosition = transform.position + transform.forward * attackRange * 0.5f;

        // Spawn warning zone prefab
        if (warningZonePrefab != null)
        {
            currentWarningZone = Instantiate(warningZonePrefab, warnPosition, transform.rotation);
        }

        // Play preparation sound if assigned
        if (audioSource != null && prepSound != null)
            audioSource.PlayOneShot(prepSound);

        Debug.Log("SlashAttack preparation phase...");
        yield return new WaitForSeconds(preparationTime);

        // Remove warning zone before attack
        if (currentWarningZone != null)
            Destroy(currentWarningZone);

        // Play attack sound if assigned
        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        // Clear damaged targets before attack starts
        damagedTargets.Clear();

        float elapsed = 0f;

        // Attack active for attackDuration seconds
        while (elapsed < attackDuration)
        {
            Vector3 attackCenter = transform.position + transform.forward * attackRange * 0.5f;
            float radius = attackRange * 0.5f;

            // Detect all colliders inside the damage sphere
            Collider[] hits = Physics.OverlapSphere(attackCenter, radius);

            foreach (var hit in hits)
            {
                // Skip if this target already damaged in this attack cycle
                if (damagedTargets.Contains(hit.gameObject))
                    continue;

                HPSystem hp = hit.GetComponentInParent<HPSystem>();
                if (hp != null)
                {
                    // Calculate knockback direction, flatten on y-axis
                    Vector3 knockbackDir = (hit.transform.position - transform.position).normalized;
                    knockbackDir.y = 0f;

                    // Deal damage and apply knockback force
                    hp.TakeDamage((int)damage, knockbackDir, knockbackForce);

                    // Mark this target as damaged to avoid repeated hits
                    damagedTargets.Add(hit.gameObject);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Parameterless ExecuteCoroutine method required by IAttackPattern interface.
    /// Calls ExecuteCoroutine with null target.
    /// </summary>
    /// <returns>Coroutine IEnumerator</returns>
    public IEnumerator ExecuteCoroutine()
    {
        return ExecuteCoroutine(null);
    }
}
