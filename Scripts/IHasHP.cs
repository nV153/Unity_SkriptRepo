using UnityEngine;

/// <summary>
/// Interface for objects that have health points (HP) and can take damage or be healed.
/// </summary>
public interface IHasHP
{
    /// <summary>
    /// Current health points of the object.
    /// </summary>
    int HP { get; set; }

    /// <summary>
    /// Applies damage to the object, reducing its HP.
    /// </summary>
    /// <param name="dmg">Amount of damage to apply.</param>
    void TakeDamage(int dmg);

    /// <summary>
    /// Applies damage to the object with knockback effect.
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    /// <param name="knockbackDirection">Direction of the knockback force.</param>
    /// <param name="force">Strength of the knockback force.</param>
    void TakeDamage(int damage, Vector3 knockbackDirection, float force);

    /// <summary>
    /// Heals the object, increasing its HP.
    /// </summary>
    /// <param name="heala">Amount of HP to restore.</param>
    void Heal(int heala);
}
