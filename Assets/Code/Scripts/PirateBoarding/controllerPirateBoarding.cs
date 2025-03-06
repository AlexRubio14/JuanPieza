using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class controllerPirateBoarding : MonoBehaviour
{
    public bool isBoarding = false;

    private Vector3 posToJump;
    [SerializeField] private float jumpHeigt;
    
    private float parabolaProcess = 0f;
    [SerializeField] private float parabolaSpeed;

    [SerializeField] private float knockbackForce;


    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Vector3 targetPos;
    public enum PirateState { WAITING, PARABOLA, CHASING, DEAD }

    public PirateState currentState { get; private set; } = PirateState.WAITING;

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
            case PirateState.WAITING:
                break;
            case PirateState.PARABOLA:
                JumpIntoPlayerShip();
                break;
            case PirateState.CHASING:
                CalculateTarget();
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
            ActivateNavMesh();
            ChangeState(PirateState.CHASING);
        }
    }
    
    public void ChangeState(PirateState newState)
    {

        switch (currentState)
        {
            case PirateState.WAITING:
                break;
            case PirateState.PARABOLA:
                break;
            case PirateState.CHASING:
                break;
            case PirateState.DEAD:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case PirateState.WAITING:
                break;
            case PirateState.PARABOLA:
                JumpIntoPlayerShip();
                break;
            case PirateState.CHASING:
                break;
            case PirateState.DEAD:
                rb.isKinematic = true;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void ActivateNavMesh()
    {
        navMeshAgent.enabled = true;
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

    private void CalculateTarget()
    {
        Vector3 tempTargetPos = Vector3.zero;
        float tempDistance = 100;

        NavMeshPath currentPath = new NavMeshPath();

        foreach(PlayerController controller in PlayersManager.instance.ingamePlayers)
        {
            navMeshAgent.CalculatePath(controller.transform.position, currentPath);

            if (currentPath.status == NavMeshPathStatus.PathInvalid)
                continue;

            Vector3 disFromPirateToPlayer = controller.transform.position - transform.position;

            if(disFromPirateToPlayer.magnitude <= tempDistance)
            {
                tempTargetPos = controller.transform.position;
                tempDistance = disFromPirateToPlayer.magnitude;
            }
        }

        if (tempTargetPos == Vector3.zero)
            tempTargetPos = transform.position;

        navMeshAgent.SetDestination(tempTargetPos);
    }

    public void SetPirateToJump()
    {
        CalculateNearPointToJump();
        ChangeState(PirateState.PARABOLA);
    }

    public void ResetPirate()
    {
        transform.SetParent(ManagerPirateBoarding.Instance.piratesHolder, true);
        transform.position = ManagerPirateBoarding.Instance.piratesHolder.transform.position;
        currentState = PirateState.WAITING;
        isBoarding = false;
        rb.isKinematic = true;
    }

    public void SetPosToJump(Vector3 _posToJump)
    {
        posToJump = _posToJump;
    }

    private void KnockbackPlayer(GameObject player)
    {
        Vector3 knockbackDir = transform.forward;
        knockbackDir.y = 1;

        if(player.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(knockbackForce * knockbackDir.normalized, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            KnockbackPlayer(collision.gameObject);
        }
    }
}
