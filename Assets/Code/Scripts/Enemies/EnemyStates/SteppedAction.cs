using System;
using UnityEngine;

public abstract class SteppedAction : EnemyAction
{
    protected enum ActionState { GOING_RESOURCE, WAITING_RESOURCE, GOING_TARGET, INTERACTING }
    protected ActionState currentState = ActionState.GOING_RESOURCE;

    public enum ResourceType { WOOD, BULLET, BARREL, FUEL, HAMMER }
    protected ResourceType resouceType;


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
                GoingResource();
                break;
            case ActionState.WAITING_RESOURCE:
                WaitingResource();
                break;
            case ActionState.GOING_TARGET:
                GoingTarget();
                break;
            case ActionState.INTERACTING:
                Interacting();
                break;
            default:
                break;
        }
    }

    protected abstract void Interact();
    private void CheckResourceObjectActive()
    {
        if (resource.isBroken)
            onActionEnd();
    }


    protected virtual void GoingResource()
    {
        agent.SetDestination(resource.transform.position);
        //Animacion de moverse a true
        if (IsNearToDestiny(resource.transform.position))
        {
            currentState = ActionState.WAITING_RESOURCE;
            agent.isStopped = true;
            //Animacion de moverse a false

            return;
        }

        CheckResourceObjectActive();
    }
    protected virtual void WaitingResource()
    {
        timeWaited += Time.deltaTime;
        if (timeWaited >= timeToGetResource)
        {
            timeWaited = 0;
            //Poner recurso en la mano
            onEnableResource(resouceType, true);
            //Activar animacion de pickUp

            //Animacion de moverse a true
        }

        CheckResourceObjectActive();
    }
    protected virtual void GoingTarget()
    {
        agent.SetDestination(target.transform.position);
        if (IsNearToDestiny(target.transform.position))
        {
            currentState = ActionState.INTERACTING;
            agent.isStopped = true;
            //Empezar animacion de reparar
            //Animacion moverse false
            //Activar particulas de reparar
            onEnableResource(resouceType, false);
        }
    }
    protected virtual void Interacting()
    {
        //Esperar para reparar
        timeWaited += Time.deltaTime;

        if (timeToInteract <= timeWaited)
        {
            //Parar animacion de interactuar
            //Reparar
            Interact();
            onActionEnd();
        }
    }

}
