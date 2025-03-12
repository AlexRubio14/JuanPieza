using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PirateBoardingController : MonoBehaviour
{
    public bool isBoarding = false;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Vector3 targetPos;

    [Space, Header("Parabola")]
    [SerializeField] private float jumpHeigt;

    [SerializeField] private float parabolaSpeed;
    private float parabolaProcess = 0f;
    private Vector3 posToJump;

    [Space, Header("Knockback")]
    [SerializeField] private float selfKnockbackForce; //Knockback a ti mismo al empujar al player

    [SerializeField] private float playerKnockbackForce;
    private bool isKnockbacking = false;

    [Space, Header("SphereCast")]
    [SerializeField] private Transform sphereCastPos;
    [SerializeField] private float radiusSphereCast;
    [SerializeField] private LayerMask playerLayer;
    public enum PirateState { WAITING, PARABOLA, CHASING, KNOCKBACK, DEAD }

    public PirateState currentState { get; private set; } = PirateState.WAITING;

    [SerializeField] private float raycastDis;
    [SerializeField] private LayerMask floorLayer;

    private float navSpeed;


    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navSpeed = navMeshAgent.speed; 
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
                CheckCanKnockbackPlayer();
                break;
            case PirateState.KNOCKBACK:
                KnockbackUpdate();
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
            case PirateState.KNOCKBACK:
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
                rb.isKinematic = true;
                navMeshAgent.speed = navSpeed;
                break;
            case PirateState.KNOCKBACK:
                navMeshAgent.speed = 0;
                navMeshAgent.enabled = false;
                rb.isKinematic = false;
                break;
            case PirateState.DEAD:
                rb.isKinematic = true;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void KnockbackUpdate()
    {
        if(isKnockbacking && rb.linearVelocity.magnitude <= Vector3.zero.magnitude)
        {
            navMeshAgent.enabled = true;
            ChangeState(PirateState.CHASING);
            isKnockbacking = false;
        }
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
            Vector3 playerPos = controller.transform.position;

            if(controller.stateMachine.currentState == controller.stateMachine.knockbackState)
            {// SI esta en knockback, queremos su punto a la altura del suelo para que se pueda hacer un camino en la mesh
                if (Physics.Raycast(playerPos, Vector3.down, out RaycastHit raycastHit, 10f, floorLayer))
                {
                    playerPos = raycastHit.point;
                }
            }

            //navMeshAgent.CalculatePath(controller.transform.position, currentPath);

            //if (currentPath.status == NavMeshPathStatus.PathInvalid)
            //    continue;

            Vector3 disFromPirateToPlayer = playerPos - transform.position;

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

    private void KnockbackPlayer(PlayerController playerController)
    {
        ChangeState(PirateState.KNOCKBACK);

        Vector3 playerKnockbackDir = transform.forward;
        playerKnockbackDir.y = 1;


        if (playerController.gameObject.TryGetComponent(out Rigidbody playerRb))
        {
            //Player knocback
            playerRb.AddForce(playerKnockbackForce * playerKnockbackDir.normalized, ForceMode.Impulse);
            playerController.stateMachine.ChangeState(playerController.stateMachine.knockbackState);

            //Pirate Knockback
            Vector3 pirateKnockbackDir = transform.forward * -1;
            PirateKnockback(pirateKnockbackDir, selfKnockbackForce);
        }
    }

    public void PirateKnockback(Vector3 _knockbackDir, float _knockbackForce)
    {
        rb.AddForce(_knockbackDir * _knockbackForce, ForceMode.Impulse);

        Invoke("IsKnockbacking", 0.3f);
    }

    private void IsKnockbacking()
    {
        isKnockbacking = true;
    }

    private void CheckCanKnockbackPlayer()
    {
        if(Physics.SphereCast(sphereCastPos.position, radiusSphereCast, transform.forward, out RaycastHit hitInfo, radiusSphereCast, playerLayer))
        {
            PlayerController playerController = hitInfo.transform.gameObject.GetComponent<PlayerController>();

            if (playerController.stateMachine.currentState == playerController.stateMachine.knockbackState)
                return;

            KnockbackPlayer(playerController);
            
        }
    }

    public void SetPirateToJump()
    {
        CalculateNearPointToJump();
        ChangeState(PirateState.PARABOLA);
    }

    public void ResetPirate()
    {
        transform.SetParent(PirateBoardingManager.Instance.piratesHolder, true);
        transform.position = PirateBoardingManager.Instance.piratesHolder.transform.position;
        ChangeState(PirateState.WAITING);
        isBoarding = false;
        rb.isKinematic = true;
        PirateBoardingManager.Instance.piratesBoarding.Remove(this);
        parabolaProcess = 0f;
        transform.forward = Vector3.forward;
    }

    public void SetPosToJump(Vector3 _posToJump)
    {
        posToJump = _posToJump;
    }

    private void OnDrawGizmos()
    {
        //Gizmo del area de empujar
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(sphereCastPos.position, radiusSphereCast);
    }
}
