using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    [SerializeField] private float raycastDis;
    [SerializeField] private LayerMask floorLayer;


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
        transform.position = FishingManager.Parabola(transform.position, posToJump, jumpHeigt, parabolaProcess);

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

        Vector3 raycastPoint = new Vector3(closestPoint.x, closestPoint.y + raycastDis, closestPoint.z);

        RaycastHit raycastHit = new RaycastHit();  

        if (Physics.Raycast(raycastPoint, Vector3.down, out raycastHit, floorLayer))
        {
            posToJump = raycastHit.point;
        }
    }

    public void SetPirateToJump()
    {
        CalculateNearPointToJump();
        ChangeState(PirateState.PARABOLA);
    }

    public void ResetPirate()
    {
        Debug.Log("Reset Pirate");

        transform.SetParent(ManagerPirateBoarding.Instance.piratesHolder, true);
        transform.position = ManagerPirateBoarding.Instance.piratesHolder.transform.position;
        currentState = PirateState.IDLE;
        isBoarding = false;
        rb.isKinematic = true;
    }

    public void SetPosToJump(Vector3 _posToJump)
    {
        posToJump = _posToJump;
    }
}
