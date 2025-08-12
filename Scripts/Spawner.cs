using UnityEngine;
using System.Collections;

/// <summary>
/// Spawns an enemy GameObject after a specified delay.
/// Ensures that only one enemy is spawned during the application's lifetime.
/// If an enemy is already spawned, no new one will be created.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// The enemy prefab to spawn.
    /// </summary>
    public GameObject enemyPrefab;

    /// <summary>
    /// The position where the enemy should spawn.
    /// </summary>
    public Vector3 spawnPosition;

    /// <summary>
    /// The rotation to apply to the spawned enemy.
    /// Defaults to Quaternion.identity (no rotation).
    /// </summary>
    public Quaternion spawnRotation = Quaternion.identity;

    /// <summary>
    /// Delay in seconds before spawning the enemy.
    /// </summary>
    public float spawnDelay = 3f;

    /// <summary>
    /// Reference to the spawned enemy to ensure only one instance exists.
    /// </summary>
    private static GameObject spawnedEnemy;

    /// <summary>
    /// Called by Unity when the script starts.
    /// If no enemy has been spawned yet, starts the delayed spawn coroutine.
    /// Otherwise, logs that an enemy already exists.
    /// </summary>
    void Start()
    {
        if (spawnedEnemy == null)
        {
            // Start spawning process with a delay
            StartCoroutine(SpawnAfterDelay());
        }
        else
        {
            Debug.Log("Enemy has already been spawned, no new spawn will occur.");
        }
    }

    /// <summary>
    /// Coroutine that waits for a delay, then spawns the enemy prefab.
    /// The spawned enemy is set to persist across scene loads.
    /// </summary>
    private IEnumerator SpawnAfterDelay()
    {
        // Wait for the specified delay before spawning
        yield return new WaitForSeconds(spawnDelay);

        // Instantiate the enemy prefab at the given position and rotation
        spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);

        // Set the spawned object's name to match the prefab's name
        spawnedEnemy.name = enemyPrefab.name;

        // Make sure the spawned enemy is not destroyed when changing scenes
        DontDestroyOnLoad(spawnedEnemy);
    }
}
