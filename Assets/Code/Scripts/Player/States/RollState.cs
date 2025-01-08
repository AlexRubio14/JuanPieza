using UnityEngine;

public class RollState : PlayerState
{
    private float rollTimePassed;
    public override void EnterState()
    {
        rollTimePassed = 0;

        controller.animator.SetTrigger("Roll");

        if (controller.movementInput != Vector2.zero)
            controller.AddImpulse(controller.movementDirection, controller.rollSpeed);
        else
            controller.AddImpulse(controller.transform.forward, controller.rollSpeed);
    }
    public override void UpdateState()
    {

        //Contar el tiempo rodando
        rollTimePassed += Time.deltaTime;
        if(rollTimePassed >= controller.rollDuration)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            ///Debug.Break();
        }
    }
    public override void FixedUpdateState()
    {
        controller.CheckSlope(controller.rollSlopeDistance, controller.rollSlopeOffset);
    }
    public override void ExitState()
    {

    }

    public override void RollAction()
    {
        //No puedes volver a rodar
    }
    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puedes usar ningun objeto*/ }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (!collision.gameObject.CompareTag("Scenario") && !collision.gameObject.CompareTag("Object"))
            return;
        //Rebotar
        Vector3 bounceDir = collision.contacts[0].normal * controller.bounceForce.x + Vector3.up * controller.bounceForce.y;
        controller.AddImpulse(bounceDir, controller.rollSpeed);
        rollTimePassed = -controller.rollDuration / 2;
        controller.animator.SetTrigger("Roll");

    }
}
