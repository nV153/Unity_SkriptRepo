using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Performs a dash attack towards a target, dealing damage and knockback to any enemies hit along the way.
/// </summary>
public class DashAttack : MonoBehaviour, IAttackPattern
{
    [Tooltip("Distance to dash forward.")]
    public float dashDistance = 10f;

    [Tooltip("Duration of the dash in seconds.")]
    public float dashDuration = 0.3f;

    [Tooltip("Damage dealt to each enemy hit.")]
    public float damage = 20f;

    [Tooltip("Force applied to knock back enemies hit by the dash.")]
    public float knockbackForce = 20f;

    [Tooltip("If true, locks the dash direction at the start of the dash.")]
    public bool lockDirectionOnDash = true;

    [Header("Sound")]
    [Tooltip("AudioSource to play dash sounds.")]
    public AudioSource audioSource;

    [Tooltip("Sound played before dashing.")]
    public AudioClip prepSound;

    [Tooltip("Sound played during the dash.")]
    public AudioClip dashSound;

    private Vector3 dashDirection;

    // Keeps track of targets already damaged during this dash
    private HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

    /// <summary>
    /// The maximum range of the dash attack.
    /// </summary>
    public float Range => dashDistance;

    /// <summary>
    /// Executes the dash attack as a coroutine, targeting a specific Transform.
    /// </summary>
    /// <param name="target">The target Transform to dash towards.</param>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        if (target == null)
            yield break;

        // Play preparation sound if available
        if (audioSource != null && prepSound != null)
            audioSource.PlayOneShot(prepSound);


        yield return new WaitForSeconds(1f); // Vorbereitungszeit

        // Calculate dash direction
        dashDirection = (target.position - transform.position).normalized;

        if (lockDirectionOnDash)
            dashDirection.y = 0f;

        dashDirection.Normalize();

        // Play dash sound if available
        if (audioSource != null && dashSound != null)
            audioSource.PlayOneShot(dashSound);

        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dashDirection * dashDistance;

        damagedTargets.Clear();

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // If not locking direction, update dash direction towards target
            if (!lockDirectionOnDash)
            {
                dashDirection = (target.position - transform.position).normalized;
                dashDirection.y = 0f;
                dashDirection.Normalize();
                targetPos = transform.position + dashDirection * dashDistance * (1 - t);
            }

            // Check for collisions with enemies within a 1m radius
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);
            foreach (var hit in hits)
            {
                if (damagedTargets.Contains(hit.gameObject)) 
                    continue;

                HPSystem hp = hit.GetComponentInParent<HPSystem>();
                if (hp != null)
                {
                    hp.TakeDamage((int)damage);
                    damagedTargets.Add(hit.gameObject);

                    Rigidbody rb = hit.attachedRigidbody;
                    var playerMovement = rb.GetComponent<PlayerMovement>();
                    if (playerMovement != null)
                    {
                        // Apply knockback using the player's movement script
                        playerMovement.ApplyKnockback(dashDirection * knockbackForce, 0.3f);
                    }
                    else
                    {
                        // Apply knockback using physics
                        rb.AddForce(dashDirection * knockbackForce, ForceMode.Impulse);
                    }
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set
        transform.position = targetPos;
    }

    /// <summary>
    /// Draws a wire sphere in the editor to visualize the dash range.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
