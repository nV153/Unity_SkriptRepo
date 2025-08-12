using UnityEngine;

/// <summary>
/// Implements a random movement pattern for characters using CharacterController.
/// Changes direction at fixed intervals and smoothly rotates toward the movement direction.
/// </summary>
public class RandomMovePattern : IMovePattern
{
    private float moveSpeed = 5f;                     // Movement speed in units per second
    private float directionChangeInterval = 1f;       // How often to change direction (seconds)
    private float timeSinceLastChange = 0f;            // Timer tracking elapsed time since last direction change
    private Vector3 direction;                          // Current movement direction (normalized)
    private float rotationSpeed = 120f;                 // Rotation speed in degrees per second

    /// <summary>
    /// Constructor initializes with a random initial direction.
    /// </summary>
    public RandomMovePattern()
    {
        ChangeDirection();
    }

    /// <summary>
    /// Moves and rotates the character according to the random pattern.
    /// </summary>
    /// <param name="characterController">The CharacterController to move.</param>
    public void Move(CharacterController characterController)
    {
        timeSinceLastChange += Time.deltaTime;

        // Change direction if interval passed
        if (timeSinceLastChange > directionChangeInterval)
        {
            ChangeDirection();
            timeSinceLastChange = 0f;
        }

        // Move character in the current direction
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        // Smoothly rotate character to face movement direction
        Vector3 newDir = Vector3.RotateTowards(
            characterController.transform.forward,
            direction,
            rotationSpeed * Mathf.Deg2Rad * Time.deltaTime,
            0.0f);

        characterController.transform.rotation = Quaternion.LookRotation(newDir);
    }

    /// <summary>
    /// Chooses a new random horizontal direction.
    /// </summary>
    private void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        direction = new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0, Mathf.Cos(randomAngle * Mathf.Deg2Rad)).normalized;
    }
}
