using UnityEngine;

/// <summary>
/// NPC controller for a ghost character that switches between idle, random movement, and chasing the player.
/// </summary>
public class GhostNPC : NPCControll
{
    private bool idlePhase = true;       // Tracks if NPC is currently idling
    private float phaseTimer = 0f;       // Timer for current movement phase
    public float idleDuration = 5f;      // Duration to stay idle
    public float randomMoveDuration = 5f;// Duration to move randomly
    public float chaseDuration = 10.0f;  // Duration to chase player before switching back to random/idle
    public Transform test;               // Player transform reference
    private bool playerFound = false;    // Flag if player detected in line of sight
    private Animator animator;           // Animator component for state animations

    protected override void Start()
    {
        base.Start();

        // Automatically find the player GameObject by tag and assign its transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            test = player.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }

        animator = GetComponent<Animator>();

        // Start with idle movement pattern and set animator state accordingly (0 = Idle)
        SetMovePattern(new IdleMovePattern());
        animator.SetInteger("state", 0);
    }

    protected override void Update()
    {
        base.Update();

        if (playerFound)
        {
            phaseTimer += Time.deltaTime;

            // After chasing for chaseDuration, switch to random movement and try to detect player again
            if (phaseTimer >= chaseDuration)
            {
                SetMovePattern(new RandomMovePattern());
                animator.SetInteger("state", 1); // 1 = RandomMove

                phaseTimer = 0f;

                GameObject playerObject;
                playerFound = ScanForPlayer(out playerObject);

                if (playerFound)
                {
                    // If player still found, resume chasing
                    SetMovePattern(new ChasePlayerMovePattern(test));
                    animator.SetInteger("state", 2); // 2 = Chase
                }
                else
                {
                    // If player lost, go back to idle
                    SetMovePattern(new IdleMovePattern());
                    animator.SetInteger("state", 0);
                }
            }
        }

        // If player not found yet, scan for player continuously
        if (!playerFound)
        {
            GameObject playerObject;
            playerFound = ScanForPlayer(out playerObject);

            if (playerFound)
            {
                SetMovePattern(new ChasePlayerMovePattern(test));
                animator.SetInteger("state", 2);
                phaseTimer = 0f;
            }
        }

        // If player still not found, alternate between idle and random movement based on timers
        if (!playerFound)
        {
            phaseTimer += Time.deltaTime;

            if (idlePhase)
            {
                // Idle phase finished, switch to random movement
                if (phaseTimer >= idleDuration)
                {
                    SetMovePattern(new RandomMovePattern());
                    animator.SetInteger("state", 1);
                    phaseTimer = 0f;
                    idlePhase = false;
                }
            }
            else
            {
                // Random movement phase finished, switch back to idle
                if (phaseTimer >= randomMoveDuration)
                {
                    SetMovePattern(new IdleMovePattern());
                    animator.SetInteger("state", 0);
                    phaseTimer = 0f;
                    idlePhase = true;
                }
            }
        }
    }
}
