using UnityEngine;

/// <summary>
/// Move pattern where an NPC chases the player smoothly,
/// moving towards the player's position and rotating to face them.
/// </summary>
public class ChasePlayerMovePattern : IMovePattern
{
    private Transform playerTransform;
    private float moveSpeed;
    private float rotationSpeed;
    private float stopDistance = 0.5f; // Minimum distance to stop moving to avoid jitter

    /// <summary>
    /// Constructor to initialize the chase move pattern.
    /// </summary>
    /// <param name="player">Transform of the player to chase.</param>
    /// <param name="moveSpeed">Movement speed of the NPC (default 5f).</param>
    /// <param name="rotationSpeed">Rotation speed in degrees per second (default 120f).</param>
    public ChasePlayerMovePattern(Transform player, float moveSpeed = 5f, float rotationSpeed = 120f)
    {
        playerTransform = player;
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    /// <summary>
    /// Moves the NPC towards the player and rotates smoothly to face them.
    /// Stops moving when within stopDistance to prevent jitter.
    /// </summary>
    /// <param name="characterController">CharacterController used to move the NPC.</param>
    public void Move(CharacterController characterController)
    {
        if (characterController == null)
        {
            Debug.LogWarning("CharacterController not assigned.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform not defined.");
            return;
        }

        // Calculate horizontal direction to player (ignore vertical difference)
        Vector3 targetDirection = playerTransform.position - characterController.transform.position;
        targetDirection.y = 0f;

        float distance = targetDirection.magnitude;

        // Move only if beyond the stopping distance
        if (distance > stopDistance)
        {
            Vector3 moveVector = targetDirection.normalized * moveSpeed * Time.deltaTime;
            characterController.Move(moveVector);
        }

        // Rotate smoothly towards the player
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            characterController.transform.rotation = Quaternion.RotateTowards(
                characterController.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
