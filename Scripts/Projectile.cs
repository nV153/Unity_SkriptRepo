using UnityEngine;

/// <summary>
/// Base class for projectiles that deal damage and self-destruct after a lifetime.
/// Designed for inheritance to customize collision behavior.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Time in seconds before the projectile destroys itself automatically.
    /// </summary>
    public float lifetime = 5f;

    /// <summary>
    /// Amount of damage dealt to targets upon collision.
    /// </summary>
    public int damage = 10;

    /// <summary>
    /// Called when the projectile is instantiated.
    /// Starts a timer to destroy the projectile after its lifetime.
    /// </summary>
    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Called when the projectile collides with another collider.
    /// Checks if the target implements IHasHP interface and applies damage.
    /// Override this method in subclasses to customize collision effects.
    /// </summary>
    /// <param name="collision">Collision information.</param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Try to get a component that can take damage
        IHasHP target = collision.collider.GetComponent<IHasHP>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Destroy the projectile after hitting
        Destroy(gameObject);
    }
}
