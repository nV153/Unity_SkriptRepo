using UnityEngine;

[CreateAssetMenu(fileName = "Claymore", menuName = "Inventory/Weapon/Claymore")]
public class ClaymoreWeapon : WeaponItem
{
    /// <summary>
    /// Overrides the hit detection to use a larger box collider for the Claymore weapon.
    /// It excludes the player's model layer to avoid self-damage.
    /// Applies knockback force to hit targets.
    /// </summary>
    /// <param name="player">The player GameObject performing the attack.</param>
    protected override void PerformHitDetection(GameObject player)
    {
        // Origin is at player hip height
        Vector3 origin = player.transform.position + new Vector3(0, 1f, 0);
        Vector3 forward = player.transform.forward;

        // Center of the hit box in front of the player
        Vector3 center = origin + forward * (attackRange / 2f);

        // Half extents of the box (wider than default to cover Claymore's range)
        Vector3 halfExtents = new Vector3(1f, 1f, attackRange / 2f);

        // Get the layer number for PlayerModel to exclude from hits
        int playerModelLayer = LayerMask.NameToLayer("PlayerModel");

        // Create a layer mask excluding the PlayerModel layer
        int layerMask = ~(1 << playerModelLayer);

        // Overlap box detects colliders in the box area, excluding PlayerModel layer
        Collider[] hits = Physics.OverlapBox(center, halfExtents, player.transform.rotation, layerMask);

        foreach (var hit in hits)
        {
            IHasHP hp = hit.GetComponent<IHasHP>();
            if (hp != null)
            {
                // Calculate horizontal direction from player to hit target for knockback
                Vector3 direction = hit.transform.position - player.transform.position;
                direction.y = 0f;
                direction.Normalize();

                // Apply damage and knockback
                hp.TakeDamage(dmg, direction, knockbackForce);
            }
        }
    }
}
