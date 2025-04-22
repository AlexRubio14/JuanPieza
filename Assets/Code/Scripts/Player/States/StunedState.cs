using UnityEngine;

public class StunedState : PlayerState
{
    public override void EnterState()
    {
        controller.currentTimeStunned = 0;
        controller.animator.SetBool("IsStunned", true);
        controller.animator.SetTrigger("Stun");
    }

    public override void ExitState()
    {
        controller.animator.SetBool("IsStunned", false);
    }

    public override void FixedUpdateState()
    {

    }

    public override void GrabAction() { }

    public override void ReleaseAction() 
    {
        controller.Release();
    }
    public override void InteractAction()
    {

    }

    public override void RollAction()
    {

    }

    public override void StopInteractAction()
    {

    }

    public override void StopUseAction()
    {

    }

    public override void UpdateState()
    {
        controller.currentTimeStunned += Time.deltaTime;
        if (controller.currentTimeStunned >= controller.maxTimeStunned)
            stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void UseAction()
    {

    }
}
