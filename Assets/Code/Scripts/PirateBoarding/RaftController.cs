using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RaftController : MonoBehaviour
{
    [SerializeField] private List<Transform> posToSpawn;

    [SerializeField] private GameObject boardingEnemy;

    [SerializeField] private List<controllerPirateBoarding> pirates;

    [SerializeField] private Rigidbody rb;

    private float direction = 1;
    [SerializeField] private float speed;

    public Vector3 destinyPos { get; private set; }
    public Vector3 startingPos { get; private set; }

    public enum RaftState { WAITING, MOVING_FRONT, MOVING_BACK, BOARDING }
    public RaftState currentState { get; private set; }
    public bool isBoarding { get; private set; } = false;

    [SerializeField] private float radiusJump;



    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case RaftState.WAITING:
                break;
            case RaftState.MOVING_FRONT:

                if(transform.position.z >= destinyPos.z)
                {
                    ChangeState(RaftState.BOARDING);
                }
                break;
            case RaftState.MOVING_BACK:

                if (transform.position.z <= startingPos.z)
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

    public void SetUpRaft()
    {
        if (isBoarding)
            return;

        int enemiesToSpawn = CalculateRandomEnemiesToSpawn();

        SetPiratesInRaft(enemiesToSpawn);

        isBoarding = true;

        List<Transform> raftStartPointsRef = managerPirateBoarding.Instance.raftStartPoints;
        int randomIndex = Random.Range(0, raftStartPointsRef.Count);

        transform.position = raftStartPointsRef[randomIndex].position;

        ChangeState(RaftState.MOVING_FRONT);
        startingPos = transform.position;
    }

    private void ResetRaft()
    {
        transform.position = transform.parent.position;

        isBoarding = false;

        pirates.Clear();
    }

    private int CalculateRandomEnemiesToSpawn()
    {
        int enemiesToSpawn = 0;

        switch (MapManager.Instance.GetCurrentHeightLevel())
        {
            case 1:
                enemiesToSpawn = Random.Range(1, 3);
                break;
            case 2:
                enemiesToSpawn = Random.Range(2, 5);
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }

        return enemiesToSpawn;
    }

    private void SetPiratesInRaft(int _enemiesToSpawn)
    {
        //If there is no available pirates Instantiate more
        if (managerPirateBoarding.Instance.piratesPool.Count == 0)
            managerPirateBoarding.Instance.InstantiatePirates();

        List<controllerPirateBoarding> pirateListRef = managerPirateBoarding.Instance.piratesPool;
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
                rb.linearVelocity = transform.forward * speed * direction;
                break;
            case RaftState.MOVING_BACK:
                direction = -1;
                rb.linearVelocity = transform.forward * speed * direction;
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
        Vector3 playerShipPos = ShipsManager.instance.playerShip.transform.position;

        foreach (controllerPirateBoarding controller in pirates)
        {
            Vector3 jumpFinalPos = playerShipPos + (Random.insideUnitSphere * radiusJump);

            jumpFinalPos.y = playerShipPos.y;

            controller.SetPosToJump(jumpFinalPos);
            controller.ChangeState(controllerPirateBoarding.PirateState.PARABOLA);
        }
    }

    public void SetDestinyPos(Vector3 pos)
    {
        destinyPos = pos;
    }
}
