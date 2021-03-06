using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PlayerStateManager))]
public class Player : MonoBehaviour
{
    [Header("Player Settings")]

    public MovementSettings movementSettings;
    public GrapplingHookSettings grappleHookSettings;
    public FlareSettings flareSettings;
    public CanvasManager playerCanvas;

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
    [HideInInspector]
    public SpriteRenderer render;
    [HideInInspector]
    public PauseMenu playerPause;

    public Collider2D collider
    {
        get
        {
            return GetComponent<Collider2D>();
        }
    }

    public Vector3 lastKnownPosition { get; set; }

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
        render = GetComponentInChildren<SpriteRenderer>();
        playerPause = GetComponent<PauseMenu>();

        playerInput.Player.Pause.performed += ctx => PauseGame();
    }
    
    public void Initialize()
    {
        stateManager.TransitionStates(PlayerStates.Movement);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerGetGrabbed += GrabbedState;
        GameEvents.OnToggleRechargingStation += OnEnterRechargingStation;
        GameEvents.OnToggleHidePlayer += HideState;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGetGrabbed -= GrabbedState;
        GameEvents.OnToggleRechargingStation -= OnEnterRechargingStation;
        GameEvents.OnToggleHidePlayer -= HideState;
    }

    private void PauseGame()
    {
        if (stateManager.GetStateInEnum(stateManager.currentState) == PlayerStates.Hiding)
            return;

        playerPause.PauseGame();
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 13 && stateManager.lastState != null)
        {
            if(stateManager.GetStateInEnum(stateManager.lastState) == PlayerStates.GrabbedByTentacle)
            {
                transform.position = lastKnownPosition;
            }
        }
    }

    private void HideState()
    {
        if (stateManager.currentState.name == "PlayerMovement")
        {
            lastKnownPosition = transform.position;
            transform.position = playerInteraction.hoveredObject.transform.position;

            stateManager.TransitionStates(PlayerStates.Hiding);
        }
        else if(stateManager.currentState.name == "PlayerHide")
        {
            transform.position = lastKnownPosition;
            stateManager.TransitionStates(PlayerStates.Movement);
        }
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

    public void SetCharacterSprite(Sprite sprite)
    {
        if (sprite == null)
            return;

        render.sprite = sprite;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

