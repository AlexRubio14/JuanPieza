using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemieManager : MonoBehaviour
{
    //Los objetos cuando se rompen llaman a las funciones del manager para ser a�adidos a la toDoList

    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform enemySpawnPoint;

    private List<EnemyController> enemyList;
    private List<EnemyAction> toDoList;
    [SerializeField]
    private EnemyAction.ActionType[] priorityList;
    [SerializeField]
    private Ship enemyShip;
    [SerializeField]
    private Ship playerShip;

    [Space, Header("Resource Spots"), SerializeField]
    private EnemyObject woodResource;
    [SerializeField]
    private EnemyObject bulletResource;
    [SerializeField]
    private EnemyObject hammerObject;

    [Space, Header("Interactions"), SerializeField]
    private float interactDistance;
    [SerializeField]
    private float timeToGetResource;
    [SerializeField]
    private float timeToRepair;
    [SerializeField]
    private float timeToInteract;

    private void Start()
    {
        enemyList = new List<EnemyController>();
        toDoList = new List<EnemyAction>();

    }

    private void OnEnable()
    {
        enemyShip.onDamageRecieved += OnDamageRecieved;
    }
    private void OnDisable()
    {
        enemyShip.onDamageRecieved -= OnDamageRecieved;
    }

    private void FixedUpdate()
    {
        foreach (EnemyController enemy in enemyList)
        {
            if (enemy.currentAction == null)
                AsignActionToEnemy(enemy, GetSomethingToDo());
        }
    }

    private void OnDamageRecieved(GameObject _hole)
    {
        AddRepairShipAction(_hole);
    }
    private EnemyAction GetSomethingToDo()
    {
        if(toDoList.Count <= 0)
            return null;

        EnemyAction greatestAction = null;
        int lastActionId = 30000;
        foreach(EnemyAction action in toDoList)
        {
            if(action.currentAction == EnemyAction.ActionType.REPAIR_SHIP)
                return action;

            int currentId = GetCurrentActionId(action.currentAction);

            if (lastActionId > currentId)
            {
                lastActionId = currentId;
                greatestAction = action;
            }
        }

        return greatestAction;
    }
    private int GetCurrentActionId(EnemyAction.ActionType _currentAcition)
    {
        for(int i = 0; i < priorityList.Length; i++)
        {
            if (priorityList[i] == _currentAcition)
            {
                return i;
            }
        }

        return 3000000;
    }

    private void AsignActionToEnemy(EnemyController _enemy, EnemyAction _action)
    {
        if(_action == null)
            return;

        _enemy.currentAction = _action;
        _action.onActionEnd += _enemy.StopAction;

        SteppedAction steppedAction = (SteppedAction)_action;
        if(steppedAction != null)
            steppedAction.onEnableResource += _enemy.EnableResource;

        _action.SetAgent(_enemy.agent);
        _action.SetAnimator(_enemy.animator);
    }

    #region Add Actions
    public void AddRepairShipAction(GameObject _hole)
    {
        EnemyAction action = new RepairShipAction(EnemyAction.ActionType.REPAIR_SHIP, transform, woodResource, _hole, SteppedAction.ResourceType.WOOD, interactDistance, timeToGetResource, timeToRepair);
        toDoList.Add(action);
    }
    public void AddShootCannonAction()
    {
        EnemyAction action = new ShootCannonAction(EnemyAction.ActionType.SHOOT_CANNON, transform, /*Parte trasera del Ca�on*/, interactDistance, timeToRepair);

        toDoList.Add(action);
    }
    public void AddReloadCannonAction()
    {
        EnemyAction action = new ReloadCannonAction(EnemyAction.ActionType.RELOAD_CANNON, transform, woodResource, /*Ca�on*/, SteppedAction.ResourceType.WOOD, interactDistance, timeToGetResource, timeToRepair);

        toDoList.Add(action);
    }
    public void AddRepairCannonAction()
    {
        EnemyAction action = new RepairCannonAction(EnemyAction.ActionType.REPAIR_CANNON, transform, woodResource, /*Ca�on*/, SteppedAction.ResourceType.WOOD, interactDistance, timeToGetResource, timeToRepair);

        toDoList.Add(action);
    }
    public void AddRepairBulletSpawnAction()
    {
        EnemyAction action = new RepairBulletSpawnerAction(EnemyAction.ActionType.REPAIR_BULLET_SPAWN, transform, woodResource, /*Spawn de balas*/ bulletResource.gameObject, SteppedAction.ResourceType.WOOD, interactDistance, timeToGetResource, timeToRepair);
        toDoList.Add(action);
    }
    #endregion

    
}
