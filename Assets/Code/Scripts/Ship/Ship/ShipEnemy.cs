
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipEnemy : Ship
{
    [Header("Velocity")]
    [SerializeField] private float velocity;
    private bool isArriving;
    private float t;
    private float startZ;
    [Space, Header("Boarding Info")]
    [SerializeField] private List<BoardShip> boardshipInformation;
    private List<BoardShip> hpBoardshipList = new List<BoardShip>();
    private List<BoardShip> initBoardshipList = new List<BoardShip>();
    [SerializeField] private HealthController enemyHealth;
    

    [SerializeField] private Transform cannonPositions;

    public override void Start()
    {
        base.Start();
        
        enemyHealth.IsEnemyHealth(true);

        t = 0;
        startZ = transform.position.z;

        foreach (BoardShip boardship in boardshipInformation)
        {
            if(boardship.spawnBoardShipCondition == BoardShip.SpawnBoardShipCondition.HP)
            {
                hpBoardshipList.Add(boardship);
                continue;
            }

            initBoardshipList.Add(boardship);
        }

        hpBoardshipList.Sort((a, b) => b.hpPercentage.CompareTo(a.hpPercentage)); // Sort hppercentages by ascdentant
        initBoardshipList.Sort((a, b) => a.timeToSpawnBoarding.CompareTo(b.timeToSpawnBoarding)); // Sort timetoSpawnRafts by descendant
    }

    protected override void Update()
    {
        base.Update();

        if (isArriving)
            Arrive();
    }

    private void Arrive()
    {
        t += Time.deltaTime * velocity;
        float newZ = Mathf.Lerp(startZ, 0, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        if (t >= 1)
        {
            foreach (var enemies in GetComponent<EnemieManager>().GetEnemyList())
            {
                //Camera.main.GetComponent<CameraController>().AddBounds(enemies.gameObject);
                enemies.GetComponent<NavMeshAgent>().enabled = true;
            }

            isArriving = false;

            foreach (BoardShip boardshipInfo in initBoardshipList)
            {
                StartCoroutine(RequestCreateRaftEvent(boardshipInfo));
            }

            AddShipBounds();
        }
    }

    private IEnumerator RequestCreateRaftEvent(BoardShip boardshipInfo)
    {
        yield return new WaitForSeconds(boardshipInfo.timeToSpawnBoarding);
        RaftManager.Instance.CreateRaftEvents(boardshipInfo);
    }
    
    public override void SetCurrentHealth(float amount)
    {
        base.SetCurrentHealth(amount);
        
        enemyHealth.SetHealthBar(GetCurrentHealth() / GetMaxHealth());
        
        if (hpBoardshipList.Count == 0)
            return;
    
        if (boardshipInformation[0].hpPercentage * 100 <= GetCurrentHealth())
        {
            RaftManager.Instance.CreateRaftEvents(boardshipInformation[0]);
            hpBoardshipList.RemoveAt(0);
        }
    }
    
    public void SetIsArriving(bool state)
    {
        isArriving = state;
    }
    
    public void SetBoardshipInformation(List<BoardShip> boardshipInfoList)
    {
        boardshipInformation = boardshipInfoList;
    }

    public Transform GetCannonPositions()
    {
        return cannonPositions;
    }
}
    