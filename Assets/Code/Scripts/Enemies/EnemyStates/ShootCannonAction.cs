using UnityEngine;

public class ShootCannonAction : EnemyAction
{

    public ShootCannonAction(ActionType _action, EnemyWeapon _target, float _distanceToInteract, float _timeToInteract)
    {
        currentAction = _action;
        target = _target;
        distanceToInteract = _distanceToInteract;
        timeToInteract = _timeToInteract;
    }

    public override void StateUpdate()
    {
        Vector3 targetPos = ((EnemyWeapon)target).weaponShootPosition.position;
        if (!IsNearToDestiny(targetPos))
        {
            //Ir hacia el arma
            agent.isStopped = false;
            agent.SetDestination(targetPos);
            animator.SetBool("Moving", true);
        }
        else
        {
            if (!agent.isStopped)
            {
                agent.isStopped = true;
                transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                Vector3 startPos = transform.position;
                Vector3 endPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                transform.forward = (endPos - startPos).normalized;
            }
            animator.SetBool("Pick", true);
            animator.SetBool("Moving", false);
            timeWaited += Time.deltaTime;

            if (timeWaited >= timeToInteract)
            {
                animator.SetTrigger("UseCanon");
                animator.SetBool("Pick", false);
                agent.isStopped = false;
                //Disparar cuando acabe el tiempo
                target.UseObject();
                onActionEnd();
            }
        }

    }
}
