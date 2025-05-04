using UnityEngine;

public class MoveState : PlayerState
{
    public override void EnterState()
    {
        controller.animator.SetBool("Moving", true);
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;
        controller.rb.useGravity = true;
    }

    public override void UpdateState()
    {
        if (controller.movementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void FixedUpdateState()
    {
        controller.Rotate(controller.movementDirection, controller.rotationSpeed);
        controller.Movement(controller.movementDirection, controller.baseMovementSpeed);
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
    }
    public override void StopUseAction() 
    { 
        controller.StopUse();
    }
    public override void PushAction()
    {
        if (!controller.objectHolder.GetHandInteractableObject())
            stateMachine.ChangeState(stateMachine.pushState);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
