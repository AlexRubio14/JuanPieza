using UnityEngine;

public class FishingState : PlayerState
{
    public FishingRod fishingRod;
    
    public override void EnterState()
    {
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
        //No puedes rodar
    }

    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() 
    {
        //Aqui deberias tener la ca�a asi que puedes usarla
        controller.Use();
    }
    public override void StopUseAction() 
    {
        controller.StopUse();    
    }

    public override void OnHit(Vector3 _hitPosition)
    {
        UseAction();
        base.OnHit(_hitPosition);
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer da�o
    }
}
