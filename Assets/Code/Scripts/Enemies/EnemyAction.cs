using System;
using UnityEngine;
using UnityEngine.AI;
public abstract class EnemyAction
{
    public enum ActionType { REPAIR_SHIP, SHOOT_CANNON, RELOAD_CANNON, REPAIR_CANNON, REPAIR_BULLET_SPAWN }
    public ActionType currentAction;
    
    protected NavMeshAgent agent;
    protected Animator animator;

    protected Transform transform;
    protected float distanceToInteract;

    protected GameObject targetObject;
    protected EnemyObject target;

    protected float timeToInteract;
    protected float timeWaited = 0;


    public Action onActionEnd;

    public abstract void StateUpdate();
    
    protected bool IsNearToDestiny(Vector3 _destiny)
    {
        //Debug.Log(Vector3.Distance(transform.position,_destiny));
        return Vector3.Distance(transform.position, _destiny) <= distanceToInteract;
    }
    public void SetAgent(NavMeshAgent _agent)
    {
        agent = _agent;
    }
    public void SetAnimator(Animator _animator)
    {
        animator = _animator;
    }
    public void SetTransform(Transform _transform)
    {
        transform = _transform;
    }


}
