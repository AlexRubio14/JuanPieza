using UnityEngine;

public class ReloadCannonAction : SteppedAction
{
    EnemyObject targetObject;
    public ReloadCannonAction(ActionType _action, Transform _transform, EnemyObject _resource, GameObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract) 
        : base(_action, _transform, _resource, _target, _resourceType, _distanceToInteract, _timeToGetResource, _timeToInteract)
    {
        targetObject = _target.GetComponent<EnemyObject>();
    }

    protected override void GoingResource()
    {
        if (!targetObject.isBroken)
            base.GoingResource();
        else
            onActionEnd();
    }
    protected override void WaitingResource()
    {
        if (!targetObject.isBroken)
            base.WaitingResource();
        else
            onActionEnd();
    }
    protected override void GoingTarget()
    {
        if(!targetObject.isBroken)
            base.GoingTarget();
        else
            onActionEnd();
    }

    protected override void Interacting()
    {
        base.Interacting();
    }

    protected override void Interact()
    {
        //Cargar el cañon
    }
}
