
using UnityEngine;

public class PushState : PlayerState
{
    public override void EnterState()
    {
        //activar animacion empujar

    }
    public override void UpdateState()
    {
        
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
        PlayerController targetController = collision.gameObject.GetComponent<PlayerController>();
        if (targetController == null)
            return;

        Debug.Log("ENTRA");
        Vector3 bounceDir = collision.contacts[0].normal * controller.pushForce.x + Vector3.up * controller.pushForce.y;
        targetController.AddImpulse(bounceDir, controller.pushSpeed);
    }
}
