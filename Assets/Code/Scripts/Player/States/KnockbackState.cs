using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState : PlayerState
{
    private Vector3 sphereCastPos = Vector3.zero;
    private float radiusSphereCast = 0.4f;

    private LayerMask objectAndScenarioLayer;

    private bool canCheckIfGrounded = false;

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
            sphereCastPos = controller.transform.position - new Vector3(0, 0.75f, 0);

            if (Physics.SphereCast(sphereCastPos, radiusSphereCast, Vector3.down,
                out RaycastHit hitInfo, radiusSphereCast, objectAndScenarioLayer))
            {
                stateMachine.ChangeState(stateMachine.idleState);
            }
        }

        currentTime += Time.deltaTime;

        //if (Physics.Raycast(controller.transform.position, Vector3.down, 1, controller.slopeCheckLayer))
        //    stateMachine.ChangeState(stateMachine.idleState);
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
