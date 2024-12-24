using UnityEngine;

public class UseState : PlayerState
{
    public override void EnterState()
    {
        //comprobar si tienes un objeto en mano
        //usar el objeto
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

    public override void PushAction()
    {
        //nada
    }

    public override void InteractAction()
    {
        //nada
    }

    public override void UseAction()
    {
        //nada
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
