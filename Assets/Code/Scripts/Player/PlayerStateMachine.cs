using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState {  get; private set; }

    public IdleState idleState {  get; private set; }
    public MoveState moveState { get; private set; }
    public RollState rollState { get; private set; }
    public PushState pushState { get; private set; }
    public InteractState interactState { get; private set; }
    public UseState useState { get; private set; }

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
        interactState = new InteractState();
        interactState.InitializeState(_controller, this);
        useState = new UseState();
        useState.InitializeState(_controller, this);

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
        if(currentState != null)
            currentState.ExitState();

        currentState = _state;

        currentState.EnterState();

        //Debug.Log("Estado actual " + currentState.ToString());
    }
}
