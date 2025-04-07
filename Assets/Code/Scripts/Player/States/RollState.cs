using UnityEngine;

public class RollState : PlayerState
{
    private float rollTimePassed;
    private bool bounced;
    private Vector3 rollDir;
    
    public override void EnterState()
    {
        rollTimePassed = 0;
        bounced = false;
        controller.animator.SetTrigger("Roll");

        if (controller.movementInput != Vector2.zero)
            rollDir = controller.movementDirection;
        else
            rollDir = controller.transform.forward;

        AudioManager.instance.Play2dOneShotSound(controller.dashClip, "Player", 1, 0.95f, 1.05f);
    }
    public override void UpdateState()
    {

        //Contar el tiempo rodando
        rollTimePassed += Time.deltaTime;
        if(rollTimePassed >= controller.rollDuration)
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void FixedUpdateState()
    {
        if(!bounced)
            controller.Movement(rollDir, controller.rollSpeed);
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
    public override void StopUseAction() { /*No hace nada*/ }

   
    public override void OnHit(Vector3 _hitPosition) { }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (bounced || !collision.gameObject.CompareTag("Scenario") && !collision.gameObject.CompareTag("Object"))
            return;

        Vector3 bounceNormal = new Vector3(collision.contacts[0].normal.x, 0, collision.contacts[0].normal.z);
        //Rebotar
        if (bounceNormal.x == 0 && bounceNormal.z == 0)
            return;

        bounced = true;
        Vector3 bounceDir = new Vector3(collision.contacts[0].normal.x, 0, collision.contacts[0].normal.z) * controller.bounceForce.x + Vector3.up * controller.bounceForce.y;
        controller.rb.linearVelocity = Vector3.zero;
        controller.AddImpulse(bounceDir, controller.rollSpeed);
        rollTimePassed = -controller.rollDuration / 2;
        controller.animator.SetTrigger("Hitted");
        AudioManager.instance.Play2dOneShotSound(controller.dashHitClip, "Player", 0.7f, 0.95f, 1.05f);
    }
}
