using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaftController : MonoBehaviour
{
    [SerializeField] private List<Transform> posToSpawn;

    [SerializeField] private GameObject boardingEnemy;

    [SerializeField] private List<controllerPirateBoarding> pirates;
    private int pirateJumpIndex = 0;
    [SerializeField] private float timeBetweenPirateJumps;

    [SerializeField] private Rigidbody rb;

    private float direction = 1;
    [SerializeField] private float speed;

    public float destinyZPos { get; private set; }
    public float startingZPos { get; private set; }

    public enum RaftState { WAITING, MOVING_FRONT, MOVING_BACK, BOARDING }
    public RaftState currentState { get; private set; }
    public bool isBoarding { get; private set; } = false;

    [SerializeField] private float radiusJump;

    private void Awake()
    {
        ChangeState(RaftState.WAITING);
    }

    private void Start()
    {
        destinyZPos = ShipsManager.instance.playerShip.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case RaftState.WAITING:
                break;
            case RaftState.MOVING_FRONT:

                if(transform.position.z >= destinyZPos)
                {
                    ChangeState(RaftState.BOARDING);
                }
                break;
            case RaftState.MOVING_BACK:

                if (transform.position.z <= startingZPos)
                {
                    ChangeState(RaftState.WAITING);
                    ResetRaft();
                    RaftManager.Instance.ProcessRaftEvent();
                }
                break;
            case RaftState.BOARDING:

                direction = -1;
                break;
            default:
                break;
        }
    }

    public void SetUpRaft(BoardShip _boardShip)
    {
        if (isBoarding)
            return;

        SetPiratesInRaft(_boardShip.enemiesCuantity);

        isBoarding = true;

        List<Transform> raftStartPointsRef = ManagerPirateBoarding.Instance.raftsStartPos;
        int randomIndex = Random.Range(0, raftStartPointsRef.Count);

        transform.position = raftStartPointsRef[randomIndex].position;

        startingZPos = transform.position.z;
        //ChangeState(RaftState.MOVING_FRONT);
    }

    private void ResetRaft()
    {
        transform.position = transform.parent.position;

        isBoarding = false;

        pirates.Clear();
        pirateJumpIndex = 0;
    }

    private void SendPirateToJump(controllerPirateBoarding _controller)
    {
        if (pirateJumpIndex == pirates.Count)
            return;

        _controller.SetPirateToJump();

        pirateJumpIndex++;

        Invoke("SendPirateToJump", timeBetweenPirateJumps);
    }

    private void SetPiratesInRaft(int _enemiesToSpawn)
    {
        //If there is no available pirates Instantiate more
        if (ManagerPirateBoarding.Instance.piratesPool.Count == 0)
            ManagerPirateBoarding.Instance.InstantiatePirates();

        List<controllerPirateBoarding> pirateListRef = ManagerPirateBoarding.Instance.piratesPool;
        int positionInRaftIndex = 0;

        foreach(controllerPirateBoarding controller in  pirateListRef)
        {
            if (controller.isBoarding)
                continue;

            pirates.Add(controller);
            controller.transform.SetParent(transform, false);
            controller.transform.position = posToSpawn[positionInRaftIndex].position;
            positionInRaftIndex++;

            controller.isBoarding = true;

            if (pirates.Count == _enemiesToSpawn)
                return;
        }
    }

    public void ChangeState(RaftState newState)
    {
        switch (currentState)
        {
            case RaftState.WAITING:
                break;
            case RaftState.MOVING_FRONT:

                break;
            case RaftState.MOVING_BACK:
                break;
            case RaftState.BOARDING:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case RaftState.WAITING:
                rb.linearVelocity = Vector3.zero;
                break;
            case RaftState.MOVING_FRONT:
                direction = 1;
                rb.linearVelocity = transform.forward * (speed * direction);
                break;
            case RaftState.MOVING_BACK:
                direction = -1;
                rb.linearVelocity = transform.forward * (speed * direction);
                break;
            case RaftState.BOARDING:
                rb.linearVelocity = Vector3.zero;
                StartBoarding();
                Invoke("StartMovingBack", 2.0f); // TODO: Change this invoke to a mehtod (When all pirates jumped into playership changeState into MOVING_BACK
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void StartMovingBack()
    {
        ChangeState(RaftState.MOVING_BACK);
    }

    private void StartBoarding()
    {
        foreach(controllerPirateBoarding controller in pirates)
        {
            controller.transform.parent = null;
        }

        //Jump into playerShip and start Chasing/boarding
        SendPirateToJump(pirates[pirateJumpIndex]);


    }

    public void SetDestinyPos(float _zPos)
    {
        destinyZPos = _zPos;
    }
}
