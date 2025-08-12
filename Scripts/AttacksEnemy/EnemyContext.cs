// EnemyContext.cs
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Central context class for an enemy that manages core components and state.
/// Provides access to NavMeshAgent, Animator, target detection, movement, attack patterns, and state handling.
/// </summary>
public class EnemyContext : MonoBehaviour
{
    /// <summary>
    /// Reference to the NavMeshAgent component for navigation.
    /// </summary>
    public NavMeshAgent Agent { get; private set; }

    /// <summary>
    /// Shortcut to this enemy's transform.
    /// </summary>
    public Transform Self => transform;

    /// <summary>
    /// Current detected target the enemy should focus on.
    /// </summary>
    public Transform Target { get; set; }

    [Header("Assign Components (Inspector)")]

    /// <summary>
    /// Reference to a MonoBehaviour implementing IDetectionMethod for detecting targets.
    /// </summary>
    public MonoBehaviour detectionScript;

    /// <summary>
    /// Reference to a MonoBehaviour implementing IMovementBehavior for movement logic.
    /// </summary>
    public MonoBehaviour movementBehaviorScript;

    /// <summary>
    /// List of MonoBehaviours implementing IAttackPattern for various attack methods.
    /// </summary>
    public List<MonoBehaviour> attackPatternScripts;

    /// <summary>
    /// Reference to the current EnemyStateHandler managing state logic.
    /// </summary>
    public EnemyStateHandler stateHandler;

    /// <summary>
    /// Strongly typed references cast from the MonoBehaviours above.
    /// </summary>
    public IDetectionMethod DetectionMethod { get; private set; }
    public IMovementBehavior MovementBehavior { get; private set; }
    public List<IAttackPattern> AttackPatterns { get; private set; }

    /// <summary>
    /// Reference to Animator for controlling animations.
    /// </summary>
    public Animator Animator { get; private set; }

    /// <summary>
    /// Flag indicating if the enemy is currently attacking.
    /// </summary>
    public bool IsAttacking { get; set; } = false;

    /// <summary>
    /// Reference to this MonoBehaviour for coroutine management, etc.
    /// </summary>
    public MonoBehaviour Mono { get; private set; }

    /// <summary>
    /// Initialize component references and cast MonoBehaviours to interfaces.
    /// </summary>
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();

        DetectionMethod = detectionScript as IDetectionMethod;
        if (DetectionMethod == null && detectionScript != null)
            Debug.LogWarning("detectionScript does not implement IDetectionMethod!");

        MovementBehavior = movementBehaviorScript as IMovementBehavior;
        if (MovementBehavior == null && movementBehaviorScript != null)
            Debug.LogWarning("movementBehaviorScript does not implement IMovementBehavior!");

        AttackPatterns = attackPatternScripts
            .Select(x => x as IAttackPattern)
            .Where(x => x != null)
            .ToList();

        if (AttackPatterns.Count == 0)
            Debug.LogWarning("No valid IAttackPattern implementations found in attackPatternScripts!");

        Mono = this;
        stateHandler = GetComponent<EnemyStateHandler>();
    }

    /// <summary>
    /// Initialize the current state handler.
    /// </summary>
    private void Start()
    {
        if (stateHandler != null)
        {
            stateHandler.Init(this);
        }
        else
        {
            Debug.LogWarning("No EnemyStateHandler assigned!");
        }
    }

    /// <summary>
    /// Helper method to run coroutines via this context.
    /// </summary>
    /// <param name="routine">IEnumerator coroutine.</param>
    /// <returns>Coroutine handle.</returns>
    public Coroutine RunCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    /// <summary>
    /// Regular update method to detect targets and update state handler.
    /// </summary>
    private void Update()
    {
        // Use detection method to find a target if available
        Target = DetectionMethod?.DetectTarget(transform);

        // Update the current state's logic
        stateHandler?.UpdateHandler();
    }

    /// <summary>
    /// Helper to set Animator boolean parameters safely.
    /// </summary>
    public void SetAnimatorBool(string name, bool value)
    {
        Animator?.SetBool(name, value);
    }

    /// <summary>
    /// Helper to set Animator trigger parameters safely.
    /// </summary>
    public void SetAnimatorTrigger(string name)
    {
        Animator?.SetTrigger(name);
    }
}
