using UnityEngine;

/// <summary>
/// Abstract base class for NPC control handling movement and player detection.
/// Uses an IMovePattern to define movement behavior.
/// </summary>
public abstract class NPCControll : MonoBehaviour
{
    protected IMovePattern movePattern;               // Movement behavior strategy
    protected CharacterController characterController; // Reference to CharacterController component
    protected float scanDistance = 20.0f;              // Distance for player detection via raycast
    public LayerMask playerLayer;                      // Layer(s) to consider as player for scanning

    /// <summary>
    /// Initializes the CharacterController reference.
    /// </summary>
    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Sets the movement pattern used by this NPC.
    /// </summary>
    /// <param name="pattern">Movement pattern implementing IMovePattern.</param>
    public void SetMovePattern(IMovePattern pattern)
    {
        movePattern = pattern;
    }

    /// <summary>
    /// Called every frame; delegates movement logic to the assigned move pattern.
    /// </summary>
    protected virtual void Update()
    {
        if (movePattern != null)
        {
            movePattern.Move(characterController);
        }
    }

    /// <summary>
    /// Scans forward for the player within scanDistance using a raycast.
    /// </summary>
    /// <param name="playerObject">Outputs the detected player GameObject if found.</param>
    /// <returns>True if player detected, false otherwise.</returns>
    public bool ScanForPlayer(out GameObject playerObject)
    {
        playerObject = null;
        RaycastHit hit;

        // Raycast forward to detect player on specified layers
        if (Physics.Raycast(transform.position, transform.forward, out hit, scanDistance, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerObject = hit.collider.gameObject;
                return true;
            }
        }
        return false;
    }
}
