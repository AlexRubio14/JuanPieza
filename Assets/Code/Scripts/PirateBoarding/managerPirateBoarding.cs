using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class managerPirateBoarding : MonoBehaviour
{
    public static managerPirateBoarding Instance { get; private set; }

    [Header("Boarding Pirates")]
    [SerializeField] private GameObject boardingEnemy;
    [SerializeField] public Transform piratesHolder;
    [SerializeField] private int piratesToSpawn;
    [SerializeField] public List<controllerPirateBoarding> piratesPool { get; private set; } = new List<controllerPirateBoarding>();

    [Header("Rafts")]
    [SerializeField] private GameObject raft;
    [SerializeField] public Transform raftsHolder;
    [SerializeField] private int raftsToSpawn;
    [SerializeField] public List<RaftController> raftsPool { get; private set; } = new List<RaftController> { };


    [Header("Raft Start Pos")]
    [SerializeField] private List<Transform> raftsStartPos = new List<Transform>();
    public List<Transform> raftStartPoints = new List<Transform>();
    [SerializeField] private float zOffset;
    [SerializeField] private Transform startPosHolder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateContent();
        CalculateRaftSpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.H)) {
            raftsPool[0].SetUpRaft();
        }
    }

    // This method should be called everytime a new stage starts 
    // TODO: Call the method CalculateRaftSpawnPoints when a stage is started
    private void CalculateRaftSpawnPoints()
    {
        int index = ShipsManager.instance.enemiesShips.Count;

        Vector3 playerShipPos = ShipsManager.instance.playerShip.transform.position;

        List<Ship> _enemiesShips = ShipsManager.instance.enemiesShips;

        for(int i = 0; i < index; i++)
        {
            Vector3 midPosBetweenShips = (playerShipPos + _enemiesShips[i].transform.position) / 2;

            raftsStartPos[i].transform.position = new Vector3(midPosBetweenShips.x, midPosBetweenShips.y, midPosBetweenShips.z - zOffset);
            raftStartPoints.Add(raftsStartPos[i]);

            raftsPool[i].SetDestinyPos(midPosBetweenShips);
        }
    }

    // Create the Pools of boarding Content in the manager, so it persists perhaps the SCENE IS CHANGED
    // TODO: Call the method InstantiateContent when a run is started
    public void InstantiateContent()
    {
        InstantiateRafts();
        InstantiatePirates();
    }

    // This method erase the pools created in the manager, BE SURE of Instantiate the content again when a run is started again
    // TODO: Call the method DeleteContent when a run is over
    public void DeleteContent()
    {
        DeletePiratePool();
        DeleteRaftPool();
    }

    public void InstantiatePirates()
    {
        for (int i = 0; i < piratesToSpawn; i++)
        {
            GameObject pirate = Instantiate(boardingEnemy, piratesHolder.position, boardingEnemy.transform.rotation);

            if (pirate.TryGetComponent(out controllerPirateBoarding controller))
            {
                piratesPool.Add(controller);
                controller.gameObject.transform.SetParent(piratesHolder, true);
            }
        }
    }

    public void InstantiateRafts()
    {
        for(int i = 0; i < raftsToSpawn; i++)
        {
            GameObject tempRaft = Instantiate(raft, raftsHolder.position, raft.transform.rotation);

            if(tempRaft.TryGetComponent(out RaftController controller))
            {
                raftsPool.Add(controller);
                controller.gameObject.transform.SetParent(raftsHolder, true);
            }
        }
    }

    public void DeletePiratePool()
    {
        foreach(controllerPirateBoarding controller in piratesPool)
        {
            if(controller != null)
            {
                Destroy(controller.gameObject);
            }
        }
    }

    public void DeleteRaftPool()
    {
        foreach(RaftController raft in raftsPool)
        {
            if(raft != null)
            {
                Destroy(raft.gameObject);
            }
        }
    }

    //This method should be called when a runs is over
    // TODO: call the method when a run is over
    void ResetRaftStartPos()
    {
        foreach(Transform gameObject in raftsStartPos)
        {
            gameObject.transform.position = startPosHolder.position;
        }
    }

    // Due to how rafts works, always will be a raft unused so we dont have to check if its null
    public RaftController GetUnusedRaft()
    {
        RaftController raftUnused = null;

        foreach (RaftController controller in raftsPool)
        {
            if (controller.isBoarding)
                return null;

            raftUnused = controller;
        }

        return raftUnused;
    }
}
