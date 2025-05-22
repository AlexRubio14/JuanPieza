using UnityEngine;

public class RepairBulletSpawnerAction : SteppedAction 
{
    public RepairBulletSpawnerAction
        (ActionType _action, EnemyObject _resource, EnemyObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract) 
        : base(_action, _resource, _target, _resourceType, _distanceToInteract, _timeToGetResource, _timeToInteract)
    {
    }

    protected override void Interact()
    {
        target.FixObject();
    }
}
