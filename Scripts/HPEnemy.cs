using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemy health system implementing IHasHP interface.
/// Manages HP, damage with knockback, healing, and death behavior.
/// </summary>
public class HPEnemy : MonoBehaviour, IHasHP
{
    /// <summary>
    /// Current health points of the enemy.
    /// </summary>
    public int hp = 100;

    /// <summary>
    /// Flag indicating whether the enemy is dead.
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// Duration of the death animation (in seconds).
    /// </summary>
    public float deadAnimationTime = 0f;

    private void Start()
    {
        // Initialization if needed
    }

    private void Update()
    {
        // Check for death condition
        if (HP <= 0 && !isDead)
        {
            Die();
        }
    }

    /// <summary>
    /// Applies damage to the enemy, reducing HP.
    /// </summary>
    /// <param name="dmg">Amount of damage.</param>
    public void TakeDamage(int dmg)
    {
        Debug.Log("Enemy took damage: " + dmg);
        hp -= dmg;
    }

    /// <summary>
    /// Applies damage with knockback force.
    /// Temporarily disables kinematic Rigidbody and NavMeshAgent during knockback.
    /// </summary>
    /// <param name="damage">Amount of damage.</param>
    /// <param name="knockbackDirection">Direction of knockback force.</param>
    /// <param name="force">Strength of knockback force.</param>
    public void TakeDamage(int damage, Vector3 knockbackDirection, float force)
    {
        TakeDamage(damage);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            bool wasKinematic = rb.isKinematic;
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            bool hadAgent = agent != null;

            if (wasKinematic)
            {
                // Temporarily make Rigidbody non-kinematic to allow physics response
                rb.isKinematic = false;
                if (hadAgent) agent.enabled = false;
            }

            rb.AddForce(knockbackDirection.normalized * force, ForceMode.Impulse);

            StartCoroutine(ResetAfterKnockback(rb, agent, wasKinematic, hadAgent));
        }
    }

    /// <summary>
    /// Coroutine to reset Rigidbody and NavMeshAgent states after knockback.
    /// </summary>
    private IEnumerator ResetAfterKnockback(Rigidbody rb, UnityEngine.AI.NavMeshAgent agent, bool wasKinematic, bool hadAgent)
    {
        yield return new WaitForSeconds(0.3f); // Knockback duration

        rb.velocity = Vector3.zero;

        if (wasKinematic)
        {
            rb.isKinematic = true;
            if (hadAgent && agent != null) agent.enabled = true;
        }
    }

    /// <summary>
    /// Heals the enemy, clamping HP to max 100.
    /// </summary>
    /// <param name="heala">Amount to heal.</param>
    public void Heal(int heala)
    {
        hp += heala;
        hp = Mathf.Clamp(hp, 0, 100);
    }

    /// <summary>
    /// Gets or sets current HP.
    /// </summary>
    public int HP
    {
        get => hp;
        set => hp = value;
    }

    /// <summary>
    /// Handles death logic: marks as dead and destroys the enemy GameObject.
    /// Override this method in subclasses to add custom death behavior.
    /// </summary>
    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
