using UnityEngine;

public class RepairShipAction : SteppedAction
{
    public RepairShipAction(ActionType _action, EnemyObject _resource, GameObject _targetObject, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract) 
        : base(_action, _resource, _targetObject, _resourceType, _distanceToInteract, _timeToGetResource, _timeToInteract)
    {
        
    }

    protected override void Interact()
    {
        GameObject.Destroy(targetObject);
    }

}
