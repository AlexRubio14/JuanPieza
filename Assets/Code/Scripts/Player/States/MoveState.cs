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
                //Esto soluciona el problema de cuando te mueves en horizontal por encima de una rampa la gravedad te empuja hacia abajo
                controller.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; 
                moveDir /= 2; //Lo divido entre 2 para que el movimiento en horizontal en medio de la rampa no vaya tan rapido
            }
        }
        else
            moveDir = controller.movementDirection;

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
