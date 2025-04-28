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
    public abstract void GrabAction();
    public abstract void ReleaseAction();
    public abstract void InteractAction();
    public abstract void StopInteractAction();
    public abstract void UseAction();
    public abstract void StopUseAction();
    public virtual void OnHit(Vector3 _hitPosition, float forceMultiplier = 1)
    {
        Vector3 knockbackDirection = (controller.transform.position - _hitPosition).normalized;

        Vector3 knockbackForce = (knockbackDirection * controller.bounceForce.x + Vector3.up * controller.bounceForce.y) * forceMultiplier;

        controller.AddImpulse(knockbackForce, controller.rollSpeed);
        controller.animator.SetTrigger("Hitted");
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            stateMachine.ChangeState(stateMachine.deathState);
        }
    }

    public virtual void OnCollisionStay(Collision collision) {}
}
