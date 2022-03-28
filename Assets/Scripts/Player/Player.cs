using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PlayerStateManager))]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]

    public MovementSettings movementSettings;
    public GrapplingHookSettings grappleHookSettings;
    public FlareSettings flareSettings;

    [HideInInspector]
    public PlayerStateManager stateManager;
    [HideInInspector]
    public TentacleController attackingTentacle;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public PlayerInventory inventory;
    [HideInInspector]
    public PlayerParanoidManager paranoidManager;
    [HideInInspector]
    public MouseLook mouseLook;
    [HideInInspector]
    public PlayerControls playerInput;
    [HideInInspector]
    public PlayerInteractionManager playerInteraction;
    [HideInInspector]
    public Light2D defaultLight;
    [HideInInspector]
    public PlayerCamera playerCam;
    [HideInInspector]
    public FlashlightController flashLight;

    public Collider2D collider
    {
        get
        {
            return GetComponent<Collider2D>();
        }
    }

    private void Awake()
    {
        playerInput = new PlayerControls();
        playerInput.Enable();

        stateManager = GetComponent<PlayerStateManager>();
        animator = GetComponentInChildren<Animator>();
        inventory = FindObjectOfType<PlayerInventory>();
        paranoidManager = GetComponent<PlayerParanoidManager>();
        mouseLook = GetComponentInChildren<MouseLook>();
        playerInteraction = GetComponent<PlayerInteractionManager>();
        defaultLight = GetComponentInChildren<Light2D>();
        playerCam = FindObjectOfType<PlayerCamera>();
        flashLight = FindObjectOfType<FlashlightController>();
    }
    
    public void Initialize()
    {
        stateManager.TransitionStates(PlayerStates.Movement);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerGetGrabbed += GrabbedState;
        GameEvents.OnToggleRechargingStation += OnEnterRechargingStation;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGetGrabbed -= GrabbedState;
        GameEvents.OnToggleRechargingStation -= OnEnterRechargingStation;
    }

    private void GrabbedState(TentacleController controller)
    {
        stateManager.TransitionStates(PlayerStates.GrabbedByTentacle);
        attackingTentacle = controller;
    }

    private void OnEnterRechargingStation(bool toggle)
    {
        flashLight.ToggleChargeBattery(toggle);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

