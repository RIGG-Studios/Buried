using UnityEngine;

public class MovementState : State
{
    private Vector2 movementInput = Vector2.zero;
    private Vector2 mouseDir = Vector2.zero;

    private Camera camera = null;
    private PlayerCamera cameraController = null;
    private MovementSettings settings = null;
    private Rigidbody2D physics = null;
    private PlayerFootsteps footSteps = null;
    private Animator animator = null;

    private float stepCooldown = 0.0f;
    private int dir = 0;

    public MovementState(Player player) : base("PlayerMovement", player)
    {
        this.player = player;
        camera = Camera.main;
        settings = player.movementSettings;
        physics = player.GetComponent<Rigidbody2D>();
        footSteps = player.GetComponent<PlayerFootsteps>();
        animator = player.animator;
        cameraController = camera.GetComponent<PlayerCamera>();
    }

    public override void EnterState()
    {
        cameraController.SetTarget(player.transform);
    }

    public override void UpdateInput()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Utilites.GetMousePosition());

        mouseDir = (mousePos - player.GetPosition()).normalized;
        movementInput = player.playerInput.Player.Move.ReadValue<Vector2>();
    }

    public override void UpdateLogic()
    {
        bool isMoving = movementInput != Vector2.zero;

        if (isMoving && stepCooldown < 0f)
        {
            footSteps.PlayFootstep();
            stepCooldown = settings.stepRate / GetMovementSpeed() / 2f;
        }

        UpdatePlayerRotation();

        cameraController.SetOffset(mouseDir * settings.cameraOffset);

        stepCooldown -= Time.deltaTime;
    }

    private void UpdatePlayerRotation()
    {
        dir = GetDirection();

        animator.SetInteger("Direction", dir);
    }

    public override void UpdatePhysics()
    {
        physics.MovePosition(physics.position + movementInput * GetMovementSpeed() * Time.fixedDeltaTime);
    }

    private float GetMovementSpeed()
    {
        float speed = settings.movementSpeed + player.paranoidManager.paranoidAmount * settings.movementParanoidMultiplier;

        return speed;
    }

    public int GetDirection()
    {
        int index = Utilites.DirectionToIndex(mouseDir, 4);

        return index;
    }
}
