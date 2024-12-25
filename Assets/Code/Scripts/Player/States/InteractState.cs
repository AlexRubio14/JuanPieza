using UnityEngine;

public class InteractState : PlayerState
{
    public override void EnterState()
    {
        //dentro de un cono hacia donde mires mirar cual es el objeto mas cercano, si hay alguno
        //coger el objeto y activar un bool en player de que llevas un objeto encima
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
        //nada
    }

    public override void OnCollisionEnter(Collision collision)
    {

    }
}
