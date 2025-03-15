using UnityEngine;

public class PushState : PlayerState
{
    private float pushTimePassed;
    public override void EnterState()
    {
        controller.animator.SetTrigger("Push");
        pushTimePassed = 0;

        Vector3 positionToCast = controller.transform.position + controller.transform.forward * controller.pushOffset;
        RaycastHit[] hits = Physics.SphereCastAll(positionToCast, controller.pushRadius, controller.transform.forward,
            1f, controller.pushLayers);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider && controller.gameObject != hit.collider.gameObject)
            {
                Vector2 pushForce = Vector2.zero;
                if (hit.collider.CompareTag("Object"))
                {
                    pushForce = controller.objectPushForce;
                    hit.rigidbody.AddTorque(controller.transform.right * controller.objectTorqueForce, ForceMode.Impulse);
                }
                else if (hit.collider && hit.collider.CompareTag("Player"))
                {
                    pushForce = controller.playerPushForce;
                    hit.collider.GetComponentInChildren<Animator>().SetTrigger("Hitted");
                }

                Vector3 pushForward = controller.transform.forward * pushForce.x;
                Vector3 pushUp = Vector3.up * pushForce.y;
                hit.rigidbody.AddForce(pushForward + pushUp, ForceMode.Impulse);
            }


            if (hit.collider && hit.collider.CompareTag("BoardingPirate"))
            {
                hit.transform.gameObject.TryGetComponent(out PirateBoardingController pirateController);
                pirateController.ChangeState(PirateBoardingController.PirateState.KNOCKBACK);
                Vector3 knockbackForward = controller.transform.forward * controller.pirateKnockbackForce;
                Vector3 pushUp = Vector3.up * controller.pirateUpForce;

                Vector3 knockbackDir = knockbackForward + pushUp;

                pirateController.PirateKnockback(knockbackDir, controller.pirateKnockbackForce);
            }
        }
    }
    public override void UpdateState()
    {
        //Contar el tiempo rodando
        pushTimePassed += Time.deltaTime;
        if (pushTimePassed >= controller.rollDuration)
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
        //No puedes rodar
    }
    public override void InteractAction() { /*No puede interactuar aqui*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puede usar nada aqui*/ }
    public override void StopUseAction() { /*No hace nada*/ }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
