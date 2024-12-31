using UnityEngine;

public abstract class PlayerState
{   
    protected PlayerController controller;
    protected PlayerStateMachine stateMachine;

    public void InitializeState(PlayerController _controller, PlayerStateMachine _stateMachine)
    {
        controller = _controller;
        stateMachine = _stateMachine;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
    
    public abstract void RollAction();
    public abstract void InteractAction();
    public abstract void UseAction();
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            stateMachine.ChangeState(stateMachine.deathState);
        }
    }

}
