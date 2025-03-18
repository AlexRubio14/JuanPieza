using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaftController : MonoBehaviour
{
    [SerializeField] private List<Transform> posToSpawn;

    [SerializeField] private GameObject boardingEnemy;

    [SerializeField] private List<PirateBoardingController> pirates;
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

    public bool eventHasFinished; // Solo lo tiene en cuenta para la mision hardcodeada de abordaje, no sirve para nada mas

    private void Awake()
    {
        ChangeState(RaftState.WAITING);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case RaftState.WAITING:
                break;
            case RaftState.MOVING_FRONT:

                if(transform.position.z >= ShipsManager.instance.playerShip.transform.position.z)
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
                if (pirateJumpIndex == pirates.Count) // Check if all pirates have jumped and moveBack the raft
                {
                    pirates.Clear();
                    ChangeState(RaftState.MOVING_BACK);
                    return;
                }
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

        List<Transform> raftStartPointsRef = PirateBoardingManager.Instance.raftsStartPos;
        int randomIndex = Random.Range(0, raftStartPointsRef.Count);

        transform.position = raftStartPointsRef[randomIndex].position;

        startingZPos = transform.position.z;
        ChangeState(RaftState.MOVING_FRONT);

    }

    public void SetUpRaftHardCoded(int piretes)
    {
        if (isBoarding)
            return;

        SetPiratesInRaft(piretes);

        isBoarding = true;

        List<Transform> raftStartPointsRef = PirateBoardingManager.Instance.raftsStartPos;

        int randomIndex = Random.Range(0, raftStartPointsRef.Count);

        transform.position = raftStartPointsRef[randomIndex].position;

        startingZPos = transform.position.z;
        ChangeState(RaftState.MOVING_FRONT);
    }

    private void ResetRaft()
    {
        transform.position = transform.parent.position;

        isBoarding = false;

        pirates.Clear();
        pirateJumpIndex = 0;

        eventHasFinished = true;
        RaftManager.Instance.ProcessRaftEvent(); //Cuando se acaba el abordaje manda una peticion al manager de si hay algun evento en cola que se procese
        RaftManager.Instance.isProcessingEvent = false; // Al acabar el evento de la raft ponemos en false que haya un evento activo
    }

    public void SendPirateToJump()
    {
        if (pirates.Count == 0)
            return;

        pirates[pirateJumpIndex].SetPirateToJump();

        pirateJumpIndex++;

        

        StartCoroutine(WaitAndSendPirateToJump()); // wait x time and send the next pirate
    }

    private IEnumerator WaitAndSendPirateToJump()
    {
        yield return new WaitForSeconds(timeBetweenPirateJumps);
        SendPirateToJump();
    }

    private void SetPiratesInRaft(int _enemiesToSpawn)
    {
        //If there is no available pirates Instantiate more
        if (PirateBoardingManager.Instance.piratesPool.Count == 0)
            PirateBoardingManager.Instance.InstantiatePirates();

        List<PirateBoardingController> pirateListRef = PirateBoardingManager.Instance.piratesPool;
        int positionInRaftIndex = 0;

        foreach(PirateBoardingController controller in  pirateListRef)
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
                eventHasFinished = false;
                break;
            case RaftState.MOVING_BACK:
                direction = -1;
                rb.linearVelocity = transform.forward * (speed * direction);
                break;
            case RaftState.BOARDING:
                rb.linearVelocity = Vector3.zero;
                StartBoarding();
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void StartBoarding()
    {
        foreach(PirateBoardingController controller in pirates)
        {
            controller.transform.parent = null;
            PirateBoardingManager.Instance.piratesBoarding.Add(controller);
        }

        //Jump into playerShip and start Chasing/boarding
        SendPirateToJump();
    }

    public void SetDestinyPos(float _zPos)
    {
        destinyZPos = _zPos;
    }

    public List<PirateBoardingController> GetPirates()
    {
        return pirates;
    }
}
