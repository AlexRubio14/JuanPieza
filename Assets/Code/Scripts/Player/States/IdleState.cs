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
    public override void GrabAction() 
    {
        controller.Grab();    
    }
    public override void ReleaseAction()
    {
        controller.Release();
    }
    public override void InteractAction()
    {
        controller.Interact();
    }
    public override void StopInteractAction() 
    {
        controller.StopInteract();
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

    
}
