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

    public override void InteractAction()
    {
        //nada
    }

    public override void UseAction()
    {
        //Recoger Anzuelo
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer daño
    }
}
