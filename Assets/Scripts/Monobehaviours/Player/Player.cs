using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStateManager))]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]

    public MovementSettings movementSettings;

    [HideInInspector]
    public PlayerStateManager stateManager;
    [HideInInspector]
    public TentacleController attackingTentacle;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerInput playerInput;

    private void Awake()
    {
        stateManager = GetComponent<PlayerStateManager>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerGetGrabbed += GrabbedState;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGetGrabbed -= GrabbedState;
    }

    private void GrabbedState(TentacleController controller)
    {
        stateManager.TransitionStates(PlayerStates.GrabbedByTentacle);
        Debug.Log(controller);
        attackingTentacle = controller;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

