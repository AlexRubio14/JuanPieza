using UnityEngine;

public class RepairCannonAction : SteppedAction
{
    public RepairCannonAction(ActionType _action, EnemyObject _resource, EnemyObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToRepair)
    : base(_action, _resource, _target, _resourceType, _distanceToInteract, _timeToGetResource, _timeToRepair)
    {
    }

    protected override void Interact()
    {
        target.FixObject();
    }
}
