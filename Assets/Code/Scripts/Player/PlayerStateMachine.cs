using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState {  get; private set; }
    public PlayerState lastState { get; private set; }
    public IdleState idleState {  get; private set; }
    public MoveState moveState { get; private set; }
    public RollState rollState { get; private set; }
    public PushState pushState { get; private set; }
    public FishingState fishingState { get; private set; }
    public KnockbackState knockbackState { get; private set; }
    public CannonState cannonState { get; private set; }
    public RepairState repairState { get; private set; }
    public DeathState deathState { get; private set; }
    public StunedState stunedState { get; private set; }
    public DrunkState drunkState { get; private set; }
    public DanceState danceState { get; private set; }

    public void InitializeStates(PlayerController _controller)
    {
        idleState = new IdleState();
        idleState.InitializeState(_controller, this);
        moveState = new MoveState();
        moveState.InitializeState(_controller, this);
        rollState = new RollState();
        rollState.InitializeState(_controller, this);
        pushState = new PushState();
        pushState.InitializeState(_controller, this);
        fishingState = new FishingState();
        fishingState.InitializeState(_controller, this);
        knockbackState = new KnockbackState();
        knockbackState.InitializeState(_controller, this);
        cannonState = new CannonState();
        cannonState.InitializeState(_controller, this);
        repairState = new RepairState();
        repairState.InitializeState(_controller, this);
        deathState = new DeathState();
        deathState.InitializeState(_controller, this);
        stunedState = new StunedState();
        stunedState.InitializeState(_controller, this);
        drunkState = new DrunkState();
        drunkState.InitializeState(_controller, this);
        danceState = new DanceState();
        danceState.InitializeState(_controller, this);

        ChangeState(idleState);
    }

    private void Update()
    {
        currentState.UpdateState();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    public void ChangeState(PlayerState _state)
    {
        if (_state == currentState)
            return;

        if(currentState != null)
            currentState.ExitState();

        lastState = currentState;
        currentState = _state;

        currentState.EnterState();
    }

    private void OnDisable()
    {
        currentState.ExitState();
    }
}
