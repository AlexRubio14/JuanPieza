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

        foreach(controllerPirateBoarding controller in pirates)
        {
            controller.transform.SetParent(managerPirateBoarding.Instance.piratesHolder, true);
            controller.transform.position = managerPirateBoarding.Instance.piratesHolder.transform.position;
        }

        pirates.Clear();

        isBoarding = false;
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
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            //If there is no available pirates Instantiate more
            if (managerPirateBoarding.Instance.piratesPool.Count == 0)
            {
                managerPirateBoarding.Instance.InstantiatePirates();
            }

            controllerPirateBoarding controller = managerPirateBoarding.Instance.piratesPool[i];

            pirates.Add(controller);
            controller.transform.SetParent(transform, false);
            controller.transform.position = posToSpawn[i].position;
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
                Invoke("StartMovingBack", 2.0f);
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void StartMovingFront()
    {
        ChangeState(RaftState.MOVING_FRONT);
    }

    private void StartMovingBack()
    {
        ChangeState(RaftState.MOVING_BACK);
    }

    private void StartBoarding()
    {
        ChangeState(RaftState.BOARDING);
    }

    public void SetDestinyPos(Vector3 pos)
    {
        destinyPos = pos;
    }

}
