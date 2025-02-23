using System.Collections.Generic;
using UnityEngine;

public class controllerPirateBoarding : MonoBehaviour
{
    public bool isBoarding = false;

    private Vector3 posToJump;
    [SerializeField] private float jumpHeigt;
    
    private float parabolaProcess = 0f;
    [SerializeField] private float parabolaSpeed;


    [SerializeField] private Rigidbody rb;

    public enum PirateState { IDLE, PARABOLA, BOARDING, DEAD }

    public PirateState currentState { get; private set; } = PirateState.IDLE;


    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)   
        {
            case PirateState.IDLE:
                break;
            case PirateState.PARABOLA:
                JumpIntoPlayerShip();
                break;
            case PirateState.BOARDING:
                break;
            case PirateState.DEAD:
                break;
            default:
                break;
        }
    }

    public void JumpIntoPlayerShip()
    {
        parabolaProcess += Time.deltaTime * parabolaSpeed;
        FishingManager.Parabola(transform.position, posToJump, transform.position.y + 5f, parabolaProcess);

        if (parabolaProcess >= 1f)
        {
            ChangeState(PirateState.IDLE);
        }

    }
    
    public void ChangeState(PirateState newState)
    {

        switch (currentState)
        {
            case PirateState.IDLE:
                break;
            case PirateState.PARABOLA:
                rb.useGravity = false;
                break;
            case PirateState.BOARDING:
                break;
            case PirateState.DEAD:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case PirateState.IDLE:
                break;
            case PirateState.PARABOLA:
                rb.useGravity = true;
                rb.isKinematic = false;
                JumpIntoPlayerShip();
                break;
            case PirateState.BOARDING:
                break;
            case PirateState.DEAD:
                rb.isKinematic = true;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    public void CalculateNearPointToJump()
    {
        List<Transform> boardingPoints = ShipsManager.instance.playerShip.boardingPoints;

        Vector3 closestPoint = Vector3.zero;
        float tempDistance = 100;

        foreach (Transform boardingPoint in boardingPoints)
        {
            Vector3 disFromPirateToPoint = boardingPoint.position - transform.position;

            if(disFromPirateToPoint.magnitude <= tempDistance)
            {
                closestPoint = boardingPoint.position;
                tempDistance = disFromPirateToPoint.magnitude;
            }
        }

        posToJump = closestPoint;
    }

    public void SetPirateToJump()
    {
        CalculateNearPointToJump();
        ChangeState(PirateState.PARABOLA);
    }

    private void ResetPirate()
    {
        //foreach (controllerPirateBoarding controller in pirates)
        //{
        //    controller.transform.SetParent(managerPirateBoarding.Instance.piratesHolder, true);
        //    controller.transform.position = managerPirateBoarding.Instance.piratesHolder.transform.position;
        //}
    }

    public void SetPosToJump(Vector3 _posToJump)
    {
        posToJump = _posToJump;
    }
}
