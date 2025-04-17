using UnityEngine;

public class IdleState : PlayerState
{

    public override void EnterState()
    {
        if (controller.movementBuffActive)
            controller.currentKnockBackTime = 0;
    }

    public override void UpdateState()
    {
        if (controller.movementInput != Vector2.zero)
            stateMachine.ChangeState(stateMachine.moveState);
    }
    public override void FixedUpdateState()
    {
        
    }
    public override void ExitState()
    {
    }

    public override void RollAction()
    {
        stateMachine.ChangeState(stateMachine.rollState);
    }
    public override void InteractAction()
    {
    }
    public override void StopInteractAction() 
    {
    }

    public override void UseAction()
    {
        if (controller.objectHolder.GetHandInteractableObject())
            controller.Use();
        else
            stateMachine.ChangeState(stateMachine.pushState);
    }
    public override void StopUseAction() 
    {
        controller.StopUse();
    }


    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override void GrabAction()
    {
        controller.Grab();

    }

    public override void ReleaseAction()
    {
        controller.Release();
    }
}
