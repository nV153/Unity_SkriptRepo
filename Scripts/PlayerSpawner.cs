using UnityEngine;

/// <summary>
/// Spawns the player GameObject once and ensures it persists across scene loads.
/// Prevents multiple player instances when switching scenes.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    /// <summary>
    /// The player prefab to spawn.
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// Position to spawn the player.
    /// </summary>
    public Vector3 spawnPosition;

    /// <summary>
    /// Rotation for the spawned player. Defaults to no rotation.
    /// </summary>
    public Quaternion spawnRotation = Quaternion.identity;

    /// <summary>
    /// Static reference to the spawned player to enforce singleton behavior.
    /// </summary>
    private static GameObject spawnedPlayer;

    /// <summary>
    /// On start, spawns the player if not already spawned.
    /// Ensures the player GameObject is not destroyed on scene changes.
    /// </summary>
    void Start()
    {
        if (spawnedPlayer == null)
        {
            // Player has not been spawned yet, instantiate now
            spawnedPlayer = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            spawnedPlayer.name = playerPrefab.name; // Optional: rename instance
            DontDestroyOnLoad(spawnedPlayer); // Keep player alive across scenes
        }
        else
        {
            Debug.Log("Player has already been spawned, no new spawn will occur.");
        }
    }
}
