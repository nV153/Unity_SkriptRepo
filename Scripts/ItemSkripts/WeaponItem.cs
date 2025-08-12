using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewWeaponItem", menuName = "Inventory/Weapon")]
public class WeaponItem : ItemData
{
    [Header("Weapon Stats")]
    public int dmg;                         // Damage dealt by the weapon attack
    public float attackRange = 2f;          // Attack range in meters
    public float attackDuration = 1f;       // Duration of the attack (e.g., animation length)
    public float attackCooldown = 0.5f;     // Cooldown time before next attack
    public float knockbackForce = 5f;       // Knockback force applied on hit

    // Example cooldown handling if needed
    // private static bool isOnCooldown = false;

    /// <summary>
    /// Called when the item is used.
    /// Finds the player and performs the attack.
    /// </summary>
    protected override void UseInternal()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        PerformHitDetection(player);
    }

    /// <summary>
    /// Coroutine for attack supporting animations or effects during the attack duration.
    /// </summary>
    private IEnumerator PerformAttack(GameObject player)
    {
        isOnCooldown = true;

        PerformHitDetection(player);

        yield return new WaitForSeconds(attackDuration);

        isOnCooldown = false;
    }

    /// <summary>
    /// Performs hit detection with a box in front of the player to detect enemies.
    /// </summary>
    /// <param name="player">The player performing the attack</param>
    protected virtual void PerformHitDetection(GameObject player)
    {
        // Origin slightly above the player's hip height
        Vector3 origin = player.transform.position + new Vector3(0, 1.0f, 0);
        Vector3 forward = player.transform.forward;

        // Box in front of the player, half the length of the attack range
        Vector3 center = origin + forward * (attackRange / 2f);
        Vector3 halfExtents = new Vector3(0.4f, 0.4f, attackRange / 2f);

        // Detect hits inside the box
        Collider[] hits = Physics.OverlapBox(center, halfExtents, player.transform.rotation);

        // Debug visualization of the box (visible for 0.2 seconds)
        DebugDrawBox(center, halfExtents, Color.yellow, 0.2f);

        // Check each hit and apply damage if it implements IHasHP
        foreach (var hit in hits)
        {
            IHasHP hp = hit.GetComponent<IHasHP>();
            if (hp != null)
            {
                hp.TakeDamage(dmg);
            }
        }
    }

    /// <summary>
    /// Draws a wireframe box for visualization in the editor or during gameplay.
    /// </summary>
    /// <param name="center">Center of the box</param>
    /// <param name="halfExtents">Half size of the box in each direction</param>
    /// <param name="color">Color of the lines</param>
    /// <param name="duration">Duration to display the lines</param>
    private void DebugDrawBox(Vector3 center, Vector3 halfExtents, Color color, float duration)
    {
        Vector3[] corners = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            float x = (i & 1) == 0 ? -1 : 1;
            float y = (i & 2) == 0 ? -1 : 1;
            float z = (i & 4) == 0 ? -1 : 1;
            corners[i] = center + Vector3.Scale(new Vector3(x, y, z), halfExtents);
        }

        // Bottom face
        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[0], corners[2], color, duration);
        Debug.DrawLine(corners[1], corners[3], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);

        // Top face
        Debug.DrawLine(corners[4], corners[5], color, duration);
        Debug.DrawLine(corners[4], corners[6], color, duration);
        Debug.DrawLine(corners[5], corners[7], color, duration);
        Debug.DrawLine(corners[6], corners[7], color, duration);

        // Vertical edges
        Debug.DrawLine(corners[0], corners[4], color, duration);
        Debug.DrawLine(corners[1], corners[5], color, duration);
        Debug.DrawLine(corners[2], corners[6], color, duration);
        Debug.DrawLine(corners[3], corners[7], color, duration);
    }
}
