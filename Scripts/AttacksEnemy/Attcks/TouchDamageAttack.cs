using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack pattern that deals damage when an enemy touches another collider,
/// either using trigger or collision events.
/// Implements a cooldown per target to avoid constant damage every frame.
/// </summary>
public class TouchDamageAttack : MonoBehaviour, IAttackPattern
{
    /// <summary>
    /// Amount of damage dealt per hit.
    /// </summary>
    public float damage = 10f;

    /// <summary>
    /// Minimum time in seconds between damage ticks on the same target.
    /// </summary>
    public float tickCooldown = 1f;

    /// <summary>
    /// If true, damage is applied via trigger events; otherwise, via collision events.
    /// </summary>
    public bool useTrigger = true;

    /// <summary>
    /// Attack range is zero because this is a contact attack.
    /// </summary>
    public float Range => 0f;

    /// <summary>
    /// Dictionary to track the last time damage was dealt to each target.
    /// Key: target GameObject, Value: last damage time in seconds.
    /// </summary>
    private Dictionary<GameObject, float> damageTimestamps = new Dictionary<GameObject, float>();

    /// <summary>
    /// Unity callback: called every frame while another collider stays inside this object's trigger collider.
    /// Deals damage if useTrigger is enabled.
    /// </summary>
    /// <param name="other">The collider of the object inside the trigger</param>
    private void OnTriggerStay(Collider other)
    {
        if (useTrigger)
            TryDealDamage(other);
    }

    /// <summary>
    /// Unity callback: called every frame while another collider stays in contact via physics collisions.
    /// Deals damage if useTrigger is disabled.
    /// </summary>
    /// <param name="collision">Collision information</param>
    private void OnCollisionStay(Collision collision)
    {
        if (!useTrigger)
            TryDealDamage(collision.collider);
    }

    /// <summary>
    /// Attempts to deal damage to the object associated with the given collider,
    /// respecting the cooldown per target.
    /// </summary>
    /// <param name="col">Collider of the object to damage</param>
    private void TryDealDamage(Collider col)
    {
        // Determine the actual GameObject to damage:
        // If collider is part of a rigidbody, use that GameObject; else, use collider's GameObject.
        GameObject target = col.attachedRigidbody != null ? col.attachedRigidbody.gameObject : col.gameObject;
        if (target == null) return;

        // Try to get a component implementing IHasHP on the target or its parents
        var hp = target.GetComponentInParent<IHasHP>();
        if (hp == null) return;

        // Check if target is still on cooldown for damage
        if (damageTimestamps.TryGetValue(target, out float lastHitTime))
        {
            if (Time.time < lastHitTime + tickCooldown)
                return; // Cooldown active, skip damage
        }

        // Apply damage
        hp.TakeDamage((int)damage);

        // Update last hit time for cooldown tracking
        damageTimestamps[target] = Time.time;
    }

    /// <summary>
    /// Executes the attack as a coroutine, required by IAttackPattern interface.
    /// This attack is event-driven, so this coroutine does nothing.
    /// </summary>
    /// <param name="target">Target transform (not used)</param>
    /// <returns>Empty IEnumerator</returns>
    public IEnumerator ExecuteCoroutine(Transform target)
    {
        yield break;
    }
}
