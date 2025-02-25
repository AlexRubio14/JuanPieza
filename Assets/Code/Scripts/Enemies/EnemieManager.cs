using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemieManager : MonoBehaviour
{
    //Los objetos cuando se rompen llaman a las funciones del manager para ser a�adidos a la toDoList

    private int totalEnemies;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject falseEnemyPrefab;
    [SerializeField]
    private Transform enemySpawnPoint;
    [SerializeField]
    private float enemySpawnOffset;

    private List<EnemyController> enemyList;
    private List<EnemyAction> toDoList;
    [SerializeField]
    private EnemyAction.ActionType[] priorityList;
    [SerializeField]
    private Ship enemyShip;

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
    [SerializeField]
    private float timeToShoot;

    private void Start()
    {
        NavMeshLink[] links = GetComponentsInChildren<NavMeshLink>();
        foreach (NavMeshLink link in links)
            link.UpdateLink();

        timeToGetResource /= NodeManager.instance.questShip.difficulty;
        timeToRepair /= NodeManager.instance.questShip.difficulty;
        timeToInteract /= NodeManager.instance.questShip.difficulty;
        timeToShoot /= NodeManager.instance.questShip.difficulty;
    }

    public void GenerateEnemies()
    {
        toDoList = new List<EnemyAction>();
        enemyList = new List<EnemyController>();

        if(totalEnemies > 0) 
        {
            for (int i = 0; i < totalEnemies; i++)
            {
                Vector3 spawnPos = enemySpawnPoint.position + new Vector3(Random.Range(-enemySpawnOffset, enemySpawnOffset), 0, Random.Range(-enemySpawnOffset, enemySpawnOffset));
                EnemyController newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<EnemyController>();
                newEnemy.enemieManager = this;
                newEnemy.gameObject.transform.SetParent(transform, true);
                newEnemy.GetComponent<NavMeshAgent>().enabled = false;

                enemyList.Add(newEnemy);
            }
        }
        else
        {
            EnemyController newEnemy = Instantiate(falseEnemyPrefab, enemySpawnPoint.position, Quaternion.identity).GetComponent<EnemyController>();
            newEnemy.enemieManager = this;
            newEnemy.gameObject.transform.SetParent(transform, true);
            //newEnemy.GetComponent<EnemyController>().currentAction.currentAction = EnemyAction.ActionType.WAIT;
            enemyList.Add(newEnemy);
        }
    }

    private void OnEnable()
    {
        enemyShip.onDamageRecieved += OnDamageRecieved;
    }
    private void OnDisable()
    {
        enemyShip.onDamageRecieved -= OnDamageRecieved;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] != null)
                Destroy(enemyList[i].gameObject);
        }

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
            {
                toDoList.Remove(action);
                return action;
            }

            int currentId = GetCurrentActionId(action.currentAction);

            if (lastActionId > currentId)
            {
                lastActionId = currentId;
                greatestAction = action;
            }
        }

        if (greatestAction != null)
            toDoList.Remove(greatestAction);
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

        if(_action is SteppedAction)
            ((SteppedAction)_action).onEnableResource += _enemy.EnableResource;

        _action.SetEnemyController(_enemy);
        _action.SetAgent(_enemy.agent);
        _action.SetAnimator(_enemy.animator);
        _action.SetTransform(_enemy.transform);
    }

    #region Add Actions
    public void AddRepairShipAction(GameObject _hole)
    {
        EnemyAction action = new RepairShipAction(EnemyAction.ActionType.REPAIR_SHIP, woodResource, _hole, SteppedAction.ResourceType.WOOD, interactDistance, timeToGetResource, timeToRepair);
        toDoList.Add(action);
    }
    public void AddShootCannonAction(EnemyWeapon _weapon)
    {
        EnemyAction action = new ShootCannonAction(EnemyAction.ActionType.SHOOT_CANNON, /*Parte trasera del Ca�on*/ _weapon, interactDistance / 2, timeToShoot);

        toDoList.Add(action);
    }
    public void AddReloadCannonAction(EnemyObject _cannon)
    {
        EnemyAction action = new ReloadCannonAction(EnemyAction.ActionType.RELOAD_CANNON, bulletResource, /*Ca�on*/_cannon, SteppedAction.ResourceType.BULLET, interactDistance, timeToGetResource, timeToInteract);

        toDoList.Add(action);
    }
    public void AddRepairCannonAction(EnemyObject _cannon)
    {
        EnemyAction action = new RepairCannonAction(EnemyAction.ActionType.REPAIR_CANNON, hammerObject, /*Ca�on*/_cannon, SteppedAction.ResourceType.HAMMER, interactDistance, timeToGetResource, timeToRepair);

        toDoList.Add(action);
    }
    public void AddRepairBulletSpawnAction()
    {
        EnemyAction action = new RepairBulletSpawnerAction(EnemyAction.ActionType.REPAIR_BULLET_SPAWN, hammerObject, /*Spawn de balas*/ bulletResource, SteppedAction.ResourceType.HAMMER, interactDistance, timeToGetResource, timeToRepair);
        toDoList.Add(action);
    }

    public void AddExistantAction(EnemyAction _action)
    {
        toDoList.Add(_action);
    }
    #endregion

    public void SetTotalEnemies(int _totalEnemies)
    {
        totalEnemies = _totalEnemies;
    }

    public List<EnemyController> GetEnemyList()
    {
        return enemyList;
    }


}
