using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Health system for player entities implementing IHasHP interface.
/// Manages HP, damage, healing, UI health bar, and knockback effects.
/// </summary>
public class HPSystem : MonoBehaviour, IHasHP
{
    /// <summary>
    /// Current health points of the entity.
    /// </summary>
    public int hp = 100;

    /// <summary>
    /// Reference to the UI Image component displaying the health bar.
    /// </summary>
    public Image healthbar;

    /// <summary>
    /// Key to trigger healing for testing/debug purposes.
    /// </summary>
    public KeyCode healKey;

    /// <summary>
    /// Key to trigger damage for testing/debug purposes.
    /// </summary>
    public KeyCode dmgKey;

    /// <summary>
    /// Reference to the PlayerMovement component to apply knockback effects.
    /// </summary>
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Start()
    {
        hp = 100; // Initialize HP to full
        UpdateHealthBar();
    }

    private void Update()
    {
        // Reload scene if health is zero or less
        if (hp <= 0)
        {
            SceneManager.LoadScene("Test");
        }

        // Debug keys to heal or damage the player
        if (Input.GetKeyDown(healKey))
        {
            Heal(5);
        }

        if (Input.GetKeyDown(dmgKey))
        {
            TakeDamage(20);
        }
    }

    /// <summary>
    /// Applies damage to the entity and updates the health bar.
    /// </summary>
    /// <param name="dmg">Amount of damage.</param>
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        hp = Mathf.Clamp(hp, 0, 100);
        UpdateHealthBar();
    }

    /// <summary>
    /// Applies damage with knockback force.
    /// </summary>
    /// <param name="damage">Amount of damage.</param>
    /// <param name="knockbackDirection">Direction of knockback.</param>
    /// <param name="force">Strength of knockback.</param>
    public void TakeDamage(int damage, Vector3 knockbackDirection, float force)
    {
        TakeDamage(damage);

        if (playerMovement != null)
        {
            playerMovement.ApplyKnockback(knockbackDirection.normalized * force, 0.3f);
        }
        else
        {
            // Fallback: apply force directly via Rigidbody if PlayerMovement not found
            Rigidbody rb = GetComponent<Rigidbody>();
            rb?.AddForce(knockbackDirection.normalized * force, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Heals the entity and updates the health bar.
    /// </summary>
    /// <param name="heala">Amount to heal.</param>
    public void Heal(int heala)
    {
        hp += heala;
        hp = Mathf.Clamp(hp, 0, 100);
        UpdateHealthBar();
    }

    /// <summary>
    /// Updates the UI health bar fill amount based on current HP.
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = hp / 100f;
        }
    }

    /// <summary>
    /// Returns the direction normal of the current collision below the object, if any.
    /// </summary>
    /// <returns>Collision normal vector or zero if no collision.</returns>
    private Vector3 CollisionDirection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            return hit.normal;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Gets or sets current HP.
    /// </summary>
    public int HP
    {
        get => hp;
        set => hp = value;
    }
}
