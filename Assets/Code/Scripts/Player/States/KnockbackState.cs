using UnityEngine;

public class KnockbackState : PlayerState
{

    public override void EnterState()
    {
        controller.animator.SetBool("Moving", false);
    }

    public override void UpdateState()
    {
        if (Physics.Raycast(controller.transform.position, Vector3.down, 1, controller.slopeCheckLayer))
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState()
    {
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public override void RollAction()
    {
    }
    public override void InteractAction()
    {
    }
    public override void StopInteractAction()
    {
    }
    public override void UseAction()
    {

    }
    public override void StopUseAction()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
