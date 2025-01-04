using System;
using UnityEngine;

public abstract class SteppedAction : EnemyAction
{
    protected enum ActionState { GOING_RESOURCE, WAITING_RESOURCE, GOING_TARGET, INTERACTING }
    protected ActionState currentState = ActionState.GOING_RESOURCE;

    public enum ResourceType { WOOD, BULLET, BARREL, FUEL }
    protected ResourceType resouceType;

    protected Transform transform;

    protected float distanceToInteract;
    protected float timeWaited = 0;

    protected EnemyObject resource;
    protected float timeToGetResource;

    protected GameObject target;
    protected float timeToInteract;


    public Action<ResourceType, bool> onEnableResource;


    public SteppedAction(ActionType _action, Transform _transform, EnemyObject _resource, GameObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract)
    {
        currentAction = _action;
        transform = _transform;
        resource = _resource;
        target = _target;
        resouceType = _resourceType;
        distanceToInteract = _distanceToInteract;
        timeToGetResource = _timeToGetResource;
        timeToInteract = _timeToInteract;
    }


    public override void StateUpdate()
    {
        if (!agent)
            return;

        switch (currentState)
        {
            case ActionState.GOING_RESOURCE:
                agent.SetDestination(resource.transform.position);
                if (IsNearToDestiny(resource.transform.position))
                {
                    currentState = ActionState.WAITING_RESOURCE;
                    agent.isStopped = true;
                }

                CheckResourceObjectActive();
                break;
            case ActionState.WAITING_RESOURCE:
                timeWaited += Time.deltaTime;
                if (timeWaited >= timeToGetResource)
                {
                    timeWaited = 0;
                    //Poner recurso en la mano
                    onEnableResource(resouceType, true);
                    //Activar animacion de pickUp
                }

                CheckResourceObjectActive();
                break;
            case ActionState.GOING_TARGET:
                agent.SetDestination(target.transform.position);
                if (IsNearToDestiny(target.transform.position))
                {
                    currentState = ActionState.INTERACTING;
                    agent.isStopped = true;
                    //Empezar animacion de reparar
                    //Activar particulas de reparar
                    //Desactivar objeto de la mano
                    onEnableResource(resouceType, false);
                }
                break;
            case ActionState.INTERACTING:
                //Esperar para reparar
                timeWaited += Time.deltaTime;

                if (timeToInteract <= timeWaited)
                {
                    //Reparar
                    Interact();
                    onActionEnd();
                }
                break;
            default:
                break;
        }
    }
    private void CheckResourceObjectActive()
    {
        if (resource.isBroken)
            onActionEnd();
    }
}
