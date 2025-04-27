using UnityEngine;

public class KnockbackState : PlayerState
{
    private Vector3 sphereCastPos = Vector3.zero;
    private float radiusSphereCast = 0.4f;

    private LayerMask objectAndScenarioLayer;

    private float timeToCheckGrounded = 0.2f;
    private float currentTime;

    public override void EnterState()
    {
        controller.animator.SetBool("Moving", false);
        objectAndScenarioLayer = controller.slopeCheckLayer | controller.objectLayer;
        currentTime = 0f;
    }

    public override void UpdateState()
    {

        if(currentTime >= timeToCheckGrounded)
        {
            if(controller.rb.linearVelocity.magnitude >= 5)
                stateMachine.ChangeState(stateMachine.idleState);
        }

        currentTime += Time.deltaTime;
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
    public override void GrabAction() { }
    public override void ReleaseAction()
    {
        controller.Release();
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
