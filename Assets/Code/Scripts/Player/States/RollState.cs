using UnityEngine;

public class RollState : PlayerState
{
    private float rollTimePassed;
    public override void EnterState()
    {
        rollTimePassed = 0;
        
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
    }
    public override void ExitState()
    {

    }

    public override void RollAction()
    {
        //No puedes volver a rodar
    }

    public override void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Rebota con " + collision.contacts[0].otherCollider.gameObject.name);
        //Rebotar
        Vector3 bounceDir = collision.contacts[0].normal * controller.bounceForce.x + Vector3.up * controller.bounceForce.y;
        controller.AddImpulse(bounceDir, controller.rollSpeed);
        rollTimePassed = -controller.rollDuration / 2;

    }
}
