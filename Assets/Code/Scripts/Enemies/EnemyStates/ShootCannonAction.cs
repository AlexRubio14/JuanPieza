using System.Collections.Generic;
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
        Vector3 targetPos = ((EnemyWeapon)target).shooterPosition.position;
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

            AimCannon();
            
            if (timeWaited >= timeToInteract)
            {
                animator.SetTrigger("Shoot");
                animator.SetBool("Pick", false);
                agent.isStopped = false;
                //Disparar cuando acabe el tiempo
                target.UseObject();
                onActionEnd(true);
            }
        }
    }

    private void AimCannon()
    {
        //Apuntar
        EnemyWeapon weapon = (EnemyWeapon)target;
        Vector3 shootPos = weapon.bulletSpawnPosition.position;


        InteractableObject nearestObject = GetNearestObject(ShipsManager.instance.playerShip.GetObjectOfType(ObjectSO.ObjectType.WEAPON), shootPos);

        if (nearestObject == null)
            nearestObject = GetNearestObject(ShipsManager.instance.playerShip.GetInventory(), shootPos);

        if (nearestObject == null)
            return;

        Vector3 weaponTargetPos = nearestObject.transform.position;

        Vector3 targetDirection = (weaponTargetPos - shootPos).normalized;
        Vector3 finalDirection = targetDirection + Vector3.up * weapon.shootHeightOffset * (Vector3.Distance(shootPos, weaponTargetPos) / 4) ;

        weapon.bulletSpawnPosition.forward = Vector3.Lerp(weapon.bulletSpawnPosition.forward, finalDirection, Time.deltaTime * weapon.aimSpeed);

    }
    private InteractableObject GetNearestObject(List<InteractableObject> _objectsToBreak, Vector3 _targetPos)
    {
        float minDistance = 1000;
        InteractableObject currentObjectToBreak = null;

        foreach (InteractableObject objectToBreak in _objectsToBreak) 
        {
            if (objectToBreak is not Repair || ((Repair)objectToBreak).GetObjectState().GetIsBroken())
                continue;

            float currentDistance = Vector3.Distance(_targetPos, objectToBreak.transform.position);
            if(currentDistance < minDistance)
            {
                currentObjectToBreak = objectToBreak;
                minDistance = currentDistance;
            }
        }


        return currentObjectToBreak;

    }
}
