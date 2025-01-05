using UnityEngine;

public class MoveState : PlayerState
{
    public override void EnterState()
    {
        controller.animator.SetBool("Moving", true);
    }

    public override void UpdateState()
    {
        if (controller.movementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void FixedUpdateState()
    {
        controller.Rotate(controller.movementDirection, controller.rotationSpeed);
        controller.CheckSlope(controller.slopeCheckDistance, controller.slopeOffset);
        controller.Movement(controller.movementDirection, controller.baseMovementSpeed);
        
    }
    public override void ExitState()
    {
        controller.animator.SetBool("Moving", false);
    }

    public override void RollAction()
    {
        stateMachine.ChangeState(stateMachine.rollState);
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
        controller.Use();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
