using System.Collections.Generic;
using UnityEngine;

public class PirateBoardingManager : MonoBehaviour
{
    public static PirateBoardingManager Instance { get; private set; }

    [Header("Boarding Pirates")]
    [SerializeField] private GameObject boardingEnemy;
    [SerializeField] public Transform piratesHolder;
    [SerializeField] private int piratesToSpawn;
    public List<PirateBoardingController> piratesBoarding { get; private set; } = new List<PirateBoardingController>();
    [SerializeField] public List<PirateBoardingController> piratesPool { get; private set; } = new List<PirateBoardingController>();

    [Header("Rafts")]
    [SerializeField] private GameObject raft;
    [SerializeField] public Transform raftsHolder;
    [SerializeField] private int raftsToSpawn;
    [SerializeField] public List<RaftController> raftsPool { get; private set; } = new List<RaftController> { };


    [Header("Raft Start Pos")]
    [field: SerializeField] public List<Transform> raftsStartPos { get; private set; } = new List<Transform>();
    [SerializeField] private float xOffset;
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

        InstantiateContent();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CalculateRaftSpawnPoints();
    }

    // This method should be called everytime a new stage starts 
    // TODO: Call the method CalculateRaftSpawnPoints when a stage is started
    private void CalculateRaftSpawnPoints()
    {
        Vector3 playerShipPos = ShipsManager.instance.playerShip.transform.position;

        raftsStartPos[0].transform.position = new Vector3(playerShipPos.x - xOffset, playerShipPos.y, playerShipPos.z -  zOffset);
        raftsStartPos[1].transform.position = new Vector3(playerShipPos.x + xOffset, playerShipPos.y, playerShipPos.z - zOffset);
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

            if (pirate.TryGetComponent(out PirateBoardingController controller))
            {
                piratesPool.Add(controller);
                controller.gameObject.transform.SetParent(piratesHolder, true);
            }
        }
    }

    public void InstantiateRafts()
    {
        for (int i = 0; i < raftsToSpawn; i++)
        {
            GameObject tempRaft = Instantiate(raft, raftsHolder.position, raft.transform.rotation);

            if (tempRaft.TryGetComponent(out RaftController controller))
            {
                raftsPool.Add(controller);
                controller.gameObject.transform.SetParent(raftsHolder, true);
            }
        }
    }

    public void DeletePiratePool()
    {
        foreach (PirateBoardingController controller in piratesPool)
        {
            if (controller != null)
            {
                Destroy(controller.gameObject);
            }
        }
    }

    public void DeleteRaftPool()
    {
        foreach (RaftController raft in raftsPool)
        {
            if (raft != null)
            {
                Destroy(raft.gameObject);
            }
        }
    }

    //This method should be called when a runs is over
    // TODO: call the method when a run is over
    void ResetRaftStartPos()
    {
        foreach (Transform gameObject in raftsStartPos)
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
            if (!controller.isBoarding)
            {
                raftUnused = controller;
                break;
            }

        }

        return raftUnused;
    }
}
