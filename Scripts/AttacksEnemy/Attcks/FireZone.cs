using UnityEngine;

/// <summary>
/// A zone effect that damages any target inside it periodically.
/// </summary>
public class FireZone : ZoneEffect
{
    /// <summary>
    /// Amount of damage applied per tick.
    /// </summary>
    public float damagePerTick = 5f;

    /// <summary>
    /// Applies the damage effect to the target GameObject.
    /// </summary>
    /// <param name="target">The target GameObject inside the zone.</param>
    protected override void ApplyToTarget(GameObject target)
    {
        // Try to get the HPSystem component from the target or its parents
        HPSystem hp = target.GetComponentInParent<HPSystem>();

        if (hp != null)
        {
            // Apply damage per tick
            hp.TakeDamage((int)damagePerTick);
        }
    }
}
