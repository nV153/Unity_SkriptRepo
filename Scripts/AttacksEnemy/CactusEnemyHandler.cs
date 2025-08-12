using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// State handler for the CactusEnemy.
/// Controls spawn animation, idle/walk/attack/dead states with NavMeshAgent movement and Animator triggers.
/// </summary>
public class CactusEnemyHandler : EnemyStateHandler
{
    [Header("Cactus Enemy Settings")]
    public float stopDistance = 3f;          // Distance to stop chasing and start attack
    public float randomWalkRadius = 10f;     // Radius for random wandering

    private bool spawnDone = false;

    public enum CactusState { Idle, Walk, Attack, Dead }
    protected CactusState currentState;

    private NavMeshAgent agent;
    private HPEnemy hpEnemy;

    /// <summary>
    /// Initialization called from EnemyContext.
    /// Sets up NavMeshAgent, HP component, triggers spawn animation and starts spawn wait coroutine.
    /// </summary>
    public override void Init(EnemyContext ctx)
    {
        base.Init(ctx);

        agent = context.Agent;  // Cache NavMeshAgent reference
        hpEnemy = context.GetComponent<HPEnemy>();  // Get HP component

        // Trigger spawn animation
        context.SetAnimatorTrigger("Spawn");

        spawnDone = false;

        // Stop agent movement during spawn
        if (agent != null)
            agent.isStopped = true;

        // Wait for spawn animation to finish
        context.RunCoroutine(WaitForSpawnEnd());
    }

    /// <summary>
    /// Coroutine that waits for spawn animation duration.
    /// After wait, enables agent and switches to Idle state.
    /// </summary>
    private IEnumerator WaitForSpawnEnd()
    {
        yield return new WaitForSeconds(6f);  // Adjust to spawn animation length

        spawnDone = true;

        if (agent != null)
            agent.isStopped = false;

        ChangeState(CactusState.Idle);
    }

    /// <summary>
    /// Update loop called each frame by EnemyContext.
    /// Handles state transitions and behaviors based on player detection and HP.
    /// </summary>
    public override void UpdateHandler()
    {
        // Wait until spawn is done
        if (!spawnDone) return;

        // Check if enemy is dead
        if (hpEnemy != null && hpEnemy.isDead)
        {
            if (currentState != CactusState.Dead)
            {
                ChangeState(CactusState.Dead);

                if (agent != null)
                    agent.isStopped = true;
            }
            return;  // No other actions when dead
        }

        var player = context.Target;

        if (player == null)
        {
            // No player detected -> random walk
            if (currentState != CactusState.Walk)
                ChangeState(CactusState.Walk);

            RandomWalk();
        }
        else
        {
            // Player detected -> chase or attack based on distance
            float distance = Vector3.Distance(context.Self.position, player.position);

            if (distance > stopDistance)
            {
                if (currentState != CactusState.Walk)
                    ChangeState(CactusState.Walk);

                agent.SetDestination(player.position);
                agent.isStopped = false;
            }
            else
            {
                if (currentState != CactusState.Attack)
                    ChangeState(CactusState.Attack);

                agent.isStopped = true;

                // TODO: Implement attack logic here (e.g. trigger damage, cooldown, etc.)
            }
        }
    }

    private float walkTimer = 0f;
    private Vector3 walkTarget;

    /// <summary>
    /// Moves enemy randomly within a radius when no player is detected.
    /// Picks a new random target periodically or upon reaching current target.
    /// </summary>
    private void RandomWalk()
    {
        walkTimer -= Time.deltaTime;

        if (walkTimer <= 0f || Vector3.Distance(context.Self.position, walkTarget) < 0.5f)
        {
            walkTimer = Random.Range(3f, 6f);

            Vector3 randomDir = Random.insideUnitSphere * randomWalkRadius;
            randomDir.y = 0;

            walkTarget = context.Self.position + randomDir;

            if (agent != null)
            {
                agent.SetDestination(walkTarget);
                agent.isStopped = false;
            }
        }
    }

    /// <summary>
    /// Changes the current state and updates animation and agent accordingly.
    /// </summary>
    /// <param name="newState">New state to switch to</param>
    protected void ChangeState(CactusState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case CactusState.Idle:
                context.SetAnimatorBool("IsWalking", false);
                if (agent != null) agent.isStopped = true;
                break;

            case CactusState.Walk:
                context.SetAnimatorBool("IsWalking", true);
                if (agent != null) agent.isStopped = false;
                break;

            case CactusState.Attack:
                context.SetAnimatorTrigger("Attack");
                if (agent != null) agent.isStopped = true;
                break;

            case CactusState.Dead:
                context.SetAnimatorTrigger("Die");
                if (agent != null) agent.isStopped = true;
                break;
        }
    }
}
