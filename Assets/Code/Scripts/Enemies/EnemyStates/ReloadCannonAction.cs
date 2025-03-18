using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ReloadCannonAction : SteppedAction
{
    public ReloadCannonAction(ActionType _action, EnemyObject _resource, EnemyObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract) 
        : base(_action, _resource, _target, _resourceType, _distanceToInteract, _timeToGetResource, _timeToInteract)
    {
    }

    protected override void GoingResource()
    {
        if (!target.isBroken)
            base.GoingResource();
        else
            onActionEnd(false);
    }
    protected override void WaitingResource()
    {
        if (!target.isBroken)
            base.WaitingResource();
        else
            onActionEnd(false);
    }
    protected override void GoingTarget()
    {
        if(!target.isBroken)
            base.GoingTarget();
        else
            onActionEnd(false);
    }

    protected override void Interacting()
    {
        if (!target.isBroken)
            base.Interacting();
        else
        {
            //Parar animacion de interactuar
            animator.SetBool("Interacting", false);
            agent.isStopped = false;
            enemyController.interactParticles.Stop(true);

            onActionEnd(false);
        }
    }

    protected override void Interact()
    {
        //Cargar el cañon
        ((EnemyWeapon)target).LoadWeapon();
    }
}
