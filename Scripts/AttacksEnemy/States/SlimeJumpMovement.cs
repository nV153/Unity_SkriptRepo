using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Movement behavior for a slime enemy that jumps periodically.
/// Uses Rigidbody to apply impulses for jumps.
/// </summary>
public class SlimeJumpMovement : MonoBehaviour, IMovementBehavior
{
    [Tooltip("Upward force applied when jumping.")]
    public float jumpForce = 7f;

    [Tooltip("Time interval between jumps in seconds.")]
    public float jumpInterval = 2f;

    [Tooltip("Forward force applied during jump.")]
    public float moveForce = 3f;

    [Tooltip("Range for random movement direction when no target.")]
    public float randomRange = 2f;

    private Rigidbody rb;
    private float lastJumpTime;
    private bool grounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogWarning("SlimeJumpMovement requires a Rigidbody component.");
    }

    private void OnCollisionStay(Collision collision)
    {
        // Simple ground detection when staying in contact with a collider
        if (collision.contacts.Length > 0)
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // No longer grounded if leaving contact with collider
        grounded = false;
    }

    /// <summary>
    /// Called each update cycle to perform movement logic.
    /// </summary>
    /// <param name="agent">NavMeshAgent (not used here)</param>
    /// <param name="self">Transform of this enemy</param>
    /// <param name="target">Target to move towards (can be null)</param>
    /// <param name="isAttacking">Whether currently attacking (not used here)</param>
    public void UpdateMovement(NavMeshAgent agent, Transform self, Transform target, bool isAttacking)
    {
        if (!grounded) return;

        if (Time.time >= lastJumpTime + jumpInterval)
        {
            lastJumpTime = Time.time;

            Vector3 direction = Vector3.zero;

            if (target != null)
            {
                // Jump towards the target, ignoring vertical axis
                direction = (target.position - self.position).normalized;
                direction.y = 0f;
            }
            else
            {
                // Random horizontal direction if no target
                direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            }

            // Calculate jump vector with upward and forward forces
            Vector3 jumpVector = direction * moveForce + Vector3.up * jumpForce;

            // Apply impulse force to Rigidbody
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }
}
