using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BurstAttack applies damage and knockback to targets within its zone effect.
/// Each target is only affected once per burst.
/// </summary>
public class BurstAttack : ZoneEffect
{
    [Tooltip("Amount of damage dealt to each target.")]
    public float damage = 30f;

    [Tooltip("Force applied to knock back each target.")]
    public float knockbackForce = 30f;

    // Keeps track of targets already affected by this burst
    private HashSet<GameObject> affectedTargets = new HashSet<GameObject>();

    /// <summary>
    /// Applies the burst effect (damage and knockback) to a target if it hasn't been affected yet.
    /// </summary>
    /// <param name="target">The GameObject to apply the effect to.</param>
    protected override void ApplyToTarget(GameObject target)
    {
        if (affectedTargets.Contains(target))
            return;

        HPSystem hp = target.GetComponentInParent<HPSystem>();
        if (hp != null)
        {
            // Calculate direction from the burst center to the target
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Use the extended TakeDamage method with knockback
            hp.TakeDamage((int)damage, direction, knockbackForce);

            affectedTargets.Add(target);
        }
    }
}
