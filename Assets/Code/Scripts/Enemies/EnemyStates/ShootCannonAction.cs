using System.Collections.Generic;
using UnityEngine;

public class ShootCannonAction : EnemyAction
{

    InteractableObject aimTargetObject;
    public ShootCannonAction(ActionType _action, EnemyWeapon _target, float _distanceToInteract, float _timeToInteract)
    {
        currentAction = _action;
        target = _target;
        distanceToInteract = _distanceToInteract;
        timeToInteract = _timeToInteract;

        aimTargetObject = GetRandomObject(ShipsManager.instance.playerShip.GetInventory());
    }

    public override void StateUpdate()
    {
        Vector3 targetPos = ((EnemyWeapon)target).shooterPosition.position;
        if (!IsNearToDestiny(targetPos))
            MoveToWeapon(targetPos);
        else
            WeaponBehaviour(targetPos);
    }

    private void MoveToWeapon(Vector3 _targetPos)
    {
        //Ir hacia el arma
        agent.isStopped = false;
        agent.SetDestination(_targetPos);
        animator.SetBool("Moving", true);
    }
    private void WeaponBehaviour(Vector3 _targetPos)
    {
        //Si sigue en movimiento pararlo en seco y hacer que mire hacia el cañon
        if (!agent.isStopped)
        {
            agent.isStopped = true;
            transform.position = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.forward = (endPos - startPos).normalized;
        }

        animator.SetBool("Pick", true);
        animator.SetBool("Moving", false);

        AimCannon();

        //Esperar a que acabe el tiempo de apuntar para disparar y acabar la accion
        timeWaited += Time.deltaTime;
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
    private void AimCannon()
    {
        //Apuntar
        EnemyWeapon weapon = (EnemyWeapon)target;
        Vector3 shootPos = weapon.bulletSpawnPosition.position;
        
        if (aimTargetObject == null)
            return;

        Vector3 weaponTargetPos = aimTargetObject.transform.position;

        Vector3 targetDirection = (weaponTargetPos - shootPos).normalized;
        Vector3 finalDirection = targetDirection + Vector3.up * weapon.shootHeightOffset * (Vector3.Distance(shootPos, weaponTargetPos) / 4) ;

        weapon.bulletSpawnPosition.forward = Vector3.Lerp(weapon.bulletSpawnPosition.forward, finalDirection, Time.deltaTime * weapon.aimSpeed);

    }
    private InteractableObject GetRandomObject(List<InteractableObject> _objectsToBreak)
    {
        return _objectsToBreak[Random.Range(0, _objectsToBreak.Count)];
    }
}
