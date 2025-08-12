using UnityEngine;

/// <summary>
/// Fireball projectile that explodes on collision, dealing area damage with knockback.
/// Inherits from Projectile base class.
/// </summary>
public class FireballProjectile : Projectile
{
    /// <summary>
    /// Radius of the explosion affecting nearby objects.
    /// </summary>
    public float explosionRadius = 3f;

    /// <summary>
    /// LayerMask to filter which objects are affected by the explosion (e.g. Player, Enemy layers).
    /// </summary>
    public LayerMask damageLayerMask;

    /// <summary>
    /// Optional particle effect prefab instantiated upon explosion.
    /// </summary>
    public GameObject explosionEffectPrefab;

    /// <summary>
    /// Strength of the knockback force applied to objects in the explosion radius.
    /// </summary>
    public float knockbackForce = 10f;

    /// <summary>
    /// Called when the fireball collides with another object.
    /// Triggers explosion effect, applies area damage and knockback, then destroys itself.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        // Instantiate explosion effect if assigned
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Get all colliders within explosion radius filtered by damageLayerMask
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);

        foreach (Collider hit in hits)
        {
            IHasHP hp = hit.GetComponent<IHasHP>();
            if (hp != null)
            {
                // Calculate knockback direction from explosion center to the target
                Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;

                // Apply damage with knockback
                hp.TakeDamage(damage, knockbackDirection, knockbackForce);

                // Debug.Log($"Explosion hits {hit.name} for {damage} damage with knockback");
            }
        }

        // Destroy the fireball object after explosion
        Destroy(gameObject);
    }

    /// <summary>
    /// Visualize explosion radius in the editor using a red wire sphere.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
