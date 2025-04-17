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
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;


        controller.Rotate(controller.movementDirection, controller.rotationSpeed);
        
        controller.rb.useGravity = true;

        Vector3 moveDir = controller.movementDirection;

        controller.Movement(moveDir, controller.baseMovementSpeed);
        
    }
    public override void ExitState()
    {
        controller.animator.SetBool("Moving", false);
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;
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
