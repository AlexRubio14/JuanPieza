using UnityEngine;

public class PushState : PlayerState
{
    private float pushTimePassed;
    public override void EnterState()
    {
        pushTimePassed = 0;

        Vector3 positionToCast = controller.transform.position + controller.transform.forward * controller.pushOffset;
        RaycastHit[] hits = Physics.SphereCastAll(positionToCast, controller.pushRadius, controller.transform.forward,
            1f, controller.pushLayers);
        foreach (RaycastHit hit in hits )
        {
            if (hit.collider && controller.gameObject != hit.collider.gameObject)
            {
                Debug.Log(hit.collider.gameObject.name);
                Vector3 pushForward = controller.transform.forward * controller.pushForce.x;
                Vector3 pushUp = Vector3.up * controller.pushForce.y;
                hit.rigidbody.AddForce(pushForward + pushUp, ForceMode.Impulse);
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
