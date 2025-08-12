using UnityEngine;

[CreateAssetMenu(fileName = "Rapier", menuName = "Inventory/Weapon/Rapier")]
public class RapierWeapon : WeaponItem
{
    /// <summary>
    /// Overrides the hit detection using a raycast instead of a box.
    /// The rapier attack targets directly forward within attack range.
    /// </summary>
    /// <param name="player">The player performing the attack</param>
    protected override void PerformHitDetection(GameObject player)
    {
        // Raycast origin at the player's hip height
        Vector3 origin = player.transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = player.transform.forward;

        // Debug visualization: red ray for the duration of the attack
        Debug.DrawRay(origin, direction * attackRange, Color.red, attackDuration);

        // Raycast forward with the attack's range
        if (Physics.Raycast(origin, direction, out RaycastHit hit, attackRange))
        {
            IHasHP hp = hit.collider.GetComponent<IHasHP>();
            if (hp != null)
            {
                hp.TakeDamage(dmg);
            }
        }
    }
}
