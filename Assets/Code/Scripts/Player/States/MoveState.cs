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

        Vector3 moveDir;
        if (controller.CheckSlope())
        {
            moveDir = controller.GetSlopeMoveDir(controller.movementDirection);
            if (moveDir.y == 0)
            {
                controller.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                moveDir /= 2;
            }
        }
        else
            moveDir = controller.movementDirection;

        controller.Movement(moveDir, controller.baseMovementSpeed);
        
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
