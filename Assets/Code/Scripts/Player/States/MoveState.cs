using UnityEngine;

public class MoveState : PlayerState
{
    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        if (controller.movementInput == Vector2.zero)
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void FixedUpdateState()
    {
        controller.Rotate(controller.movementDirection, controller.rotationSpeed);
        controller.CheckSlope();
        controller.Movement(controller.movementDirection, controller.baseMovementSpeed);
        
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
        stateMachine.ChangeState(stateMachine.interactState);
    }

    public override void UseAction()
    {
        //comprobar si tienes un objeto en mano
        //Si no tiene objeto
        stateMachine.ChangeState(stateMachine.pushState);
        //Si tiene objeto
        //stateMachine.ChangeState(stateMachine.useState);
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }

}
