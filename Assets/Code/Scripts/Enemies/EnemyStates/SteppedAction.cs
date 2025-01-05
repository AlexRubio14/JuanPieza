using System;
using UnityEngine;

public abstract class SteppedAction : EnemyAction
{
    protected enum ActionState { GOING_RESOURCE, WAITING_RESOURCE, GOING_TARGET, INTERACTING }
    protected ActionState currentState = ActionState.GOING_RESOURCE;

    public enum ResourceType { WOOD, BULLET, BARREL, FUEL, HAMMER }
    protected ResourceType resouceType;

    protected EnemyObject resource;
    protected float timeToGetResource;

    public Action<ResourceType, bool> onEnableResource;


    public SteppedAction(ActionType _action, EnemyObject _resource, EnemyObject _target, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract)
    {
        currentAction = _action;
        resource = _resource;
        target = _target;
        targetObject = null;
        resouceType = _resourceType;
        distanceToInteract = _distanceToInteract;
        timeToGetResource = _timeToGetResource;
        timeToInteract = _timeToInteract;
    }
    public SteppedAction(ActionType _action, EnemyObject _resource, GameObject _targetObject, ResourceType _resourceType, float _distanceToInteract, float _timeToGetResource, float _timeToInteract)
    {
        currentAction = _action;
        resource = _resource;
        target = null;
        targetObject = _targetObject;
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
            onActionEnd(false);
    }


    protected virtual void GoingResource()
    {
        agent.SetDestination(resource.transform.position);
        agent.isStopped = false;

        //Animacion de moverse a true
        animator.SetBool("Moving", true);
        if (IsNearToDestiny(resource.transform.position))
        {
            currentState = ActionState.WAITING_RESOURCE;
            agent.isStopped = true;
            //Animacion de moverse a false
            animator.SetBool("Moving", false);

            return;
        }

        CheckResourceObjectActive();
    }
    protected virtual void WaitingResource()
    {
        timeWaited += Time.deltaTime;
        if (timeWaited >= timeToGetResource)
        {
            currentState = ActionState.GOING_TARGET;
            timeWaited = 0;
            //Poner recurso en la mano
            onEnableResource(resouceType, true);
            //Activar animacion de pickUp
            animator.SetBool("Pick", true);
            //Animacion de moverse a true
            animator.SetBool("Moving", true);
            agent.isStopped = false;
            return;
        }

        CheckResourceObjectActive();
    }
    protected virtual void GoingTarget()
    {
        Vector3 targetPos = target? target.transform.position : targetObject.transform.position;
        agent.SetDestination(targetPos);
        if (IsNearToDestiny(targetPos))
        {
            currentState = ActionState.INTERACTING;
            agent.isStopped = true;
            //Empezar animacion de reparar
            animator.SetBool("Interacting", true);
            animator.SetBool("Pick", false);
            //Animacion moverse false
            animator.SetBool("Moving", false);
            agent.isStopped = true;
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
            animator.SetBool("Interacting", false);
            agent.isStopped = false;

            //Reparar
            Interact();
            onActionEnd(true);

        }
    }

}
