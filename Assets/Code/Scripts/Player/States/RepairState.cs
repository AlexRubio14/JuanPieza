using UnityEngine;

public class RepairState : PlayerState
{
    public override void EnterState() { }
    public override void UpdateState() { }
    public override void FixedUpdateState() { }
    public override void ExitState() { }

    public override void RollAction() { /*No puedes rodar*/ }

    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() 
    {
        //Para de reparar
        controller.StopInteract();
    }
    public override void UseAction() { /*No puedes usar ningun objeto*/ }
    public override void StopUseAction()
    {
        controller.StopUse();
    }
    public override void OnHit(Vector3 _hitPosition, float forceMultiplier = 1)
    {
        StopInteractAction();
        base.OnHit(_hitPosition);
    }
    public override void OnCollisionEnter(Collision collision)
    {
        //Si te golpea algo llamar al controller.StopInteract();
    }
}
