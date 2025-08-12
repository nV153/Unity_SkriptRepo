using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for zone effects that apply a recurring effect to objects within a trigger collider.
/// Supports optional looping sound and automatic despawning after a set time.
/// </summary>
public abstract class ZoneEffect : MonoBehaviour
{
    [Tooltip("Interval in seconds between effect applications")]
    public float interval = 1f;

    [Tooltip("Optional looping sound played while effect is active")]
    public AudioSource optionalSound;

    [Tooltip("Should this zone effect automatically despawn after a delay?")]
    public bool autoDespawn = false;

    [Tooltip("Time in seconds before automatic despawn")]
    public float despawnTime = 5f;

    // List of objects currently inside the zone
    private List<GameObject> targetsInZone = new();

    // Map active coroutines per target for clean stopping
    private Dictionary<GameObject, Coroutine> activeEffects = new();

    private void Awake()
    {
        // Optionally disable mesh renderer if any visible mesh should be hidden
        // MeshRenderer mesh = GetComponent<MeshRenderer>();
        // if (mesh != null) mesh.enabled = false;

        // Play looping sound if assigned
        if (optionalSound != null)
        {
            optionalSound.loop = true;
            optionalSound.Play();
        }

        // Start coroutine to despawn after delay if enabled
        if (autoDespawn)
        {
            StartCoroutine(DespawnAfterDelay());
        }
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add target if not already tracked and start effect coroutine
        if (!targetsInZone.Contains(other.gameObject))
        {
            targetsInZone.Add(other.gameObject);
            Coroutine co = StartCoroutine(ApplyEffect(other.gameObject));
            activeEffects[other.gameObject] = co;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop and remove effect coroutine and target on exit
        if (targetsInZone.Contains(other.gameObject))
        {
            if (activeEffects.TryGetValue(other.gameObject, out Coroutine co))
            {
                StopCoroutine(co);
                activeEffects.Remove(other.gameObject);
            }
            targetsInZone.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Coroutine that applies the effect repeatedly at the set interval.
    /// </summary>
    /// <param name="target">The target GameObject to apply effect to</param>
    private IEnumerator ApplyEffect(GameObject target)
    {
        while (true)
        {
            ApplyToTarget(target);
            yield return new WaitForSeconds(interval);
        }
    }

    /// <summary>
    /// Override to implement the actual effect applied to the target.
    /// </summary>
    /// <param name="target">The GameObject to apply the effect to</param>
    protected abstract void ApplyToTarget(GameObject target);
}
