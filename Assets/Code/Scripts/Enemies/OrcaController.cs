using UnityEngine;

public class OrcaController : MonoBehaviour
{
    private enum OrcaState { WAITING, GOING, EATING, HIDING }

    private OrcaState state;

    [SerializeField]
    private float underwaterYPos;
    [SerializeField]
    private float playerBoatRange;
    [SerializeField]
    private float orbitOffset;
    [SerializeField] 
    private float orbitSpeed;
    [SerializeField]
    private float orcaSpeed;
    [SerializeField] 
    private float orcaRotationSpeed;
    [SerializeField]
    private float distanceToEat;
    [SerializeField]
    private float eatDuration;
    [Space, SerializeField]
    private float timeToEatPlayers;

    private float orbitValue;
    private PlayerController playerToChase;
    private Vector3 posToHide;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = GetOrbitPos();
        transform.forward = GetOrbitForward();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case OrcaState.WAITING:
                OrbitShip();
                CheckPlayersOutsideBoat();
                break;
            case OrcaState.GOING:
                ChasePlayer();
                break;
            case OrcaState.EATING:
                break;
            case OrcaState.HIDING:
                CheckPlayersOutsideBoat();
                ReturnToOrbit();
                break;
            default:
                break;
        }
    }

    private void ChangeState(OrcaState _newState)
    {
        switch (state)
        {
            case OrcaState.WAITING:
                break;
            case OrcaState.GOING:
                break;
            case OrcaState.EATING:
                break;
            case OrcaState.HIDING:
                break;
            default:
                break;
        }

        state = _newState;

        switch (_newState)
        {
            case OrcaState.WAITING:
                break;
            case OrcaState.GOING:
                break;
            case OrcaState.EATING:
                (playerToChase.stateMachine.currentState as DeathState).KillPlayer();
                //Empezar animacion de comer
                animator.SetTrigger("Eat");
                Invoke("StopEat", eatDuration);
                playerToChase = null;
                break;
            case OrcaState.HIDING:
                posToHide = GetOrbitPos();
                break;
            default:
                break;
        }

    }

    private void OrbitShip()
    {
        orbitValue += orbitSpeed * Time.deltaTime;

        transform.position = GetOrbitPos();
        transform.forward = Vector3.Lerp(transform.forward, GetOrbitForward(), Time.fixedDeltaTime * orcaRotationSpeed);
    }
    private Vector3 GetOrbitPos()
    {
        return new Vector3(Mathf.Cos(orbitValue) * orbitOffset, underwaterYPos, Mathf.Sin(orbitValue) * orbitOffset);
    }
    private Vector3 GetOrbitForward()
    {
        float orbitPredictValue = orbitValue += orbitSpeed * Time.deltaTime;
        Vector3 predictedOrbitPos = new Vector3(Mathf.Cos(orbitPredictValue) * orbitOffset, underwaterYPos, Mathf.Sin(orbitPredictValue) * orbitOffset);
        Vector3 forwardOrbit = (predictedOrbitPos - transform.position).normalized;
        return forwardOrbit;
    }
    private void CheckPlayersOutsideBoat()
    {
        foreach (PlayerController player in PlayersManager.instance.ingamePlayers)
        {
            if (player.stateMachine.currentState is not DeathState || (player.stateMachine.currentState as DeathState).isDead)
                continue;

            Vector2 a = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 b = new Vector2(ShipsManager.instance.playerShip.transform.position.x, ShipsManager.instance.playerShip.transform.position.z);

            float distance = Vector2.Distance(a, b);

            if(distance >= playerBoatRange || timeToEatPlayers <= (player.stateMachine.currentState as DeathState).timeAtWater)
            {
                playerToChase = player;
                ChangeState(OrcaState.GOING);
                break;
            }
        }
    }
    private void ChasePlayer()
    {
        if (playerToChase.stateMachine.currentState is not DeathState)
        {
            ChangeState(OrcaState.HIDING);
            return;
        }

        Vector3 forwardDir = (new Vector3(playerToChase.transform.position.x, 0, playerToChase.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
        transform.forward = Vector3.Lerp(transform.forward, forwardDir, Time.fixedDeltaTime * orcaRotationSpeed);

        Vector3 newPos = transform.position + forwardDir * orcaSpeed * Time.deltaTime;
        newPos.y = underwaterYPos;
        transform.position = newPos;
        
        
        Vector2 a = new Vector2(playerToChase.transform.position.x, playerToChase.transform.position.z);
        Vector2 b = new Vector2(transform.position.x, transform.position.z);

        float distance = Vector2.Distance(a, b);
        if (distance <= distanceToEat)
        {
            ChangeState(OrcaState.EATING);
        }
    }
    private void ReturnToOrbit()
    {
        Vector3 newForward = (posToHide - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, newForward, Time.fixedDeltaTime * orcaRotationSpeed);
        transform.position += transform.forward * orcaSpeed * Time.fixedDeltaTime;

        
        Vector2 a = new Vector2(posToHide.x, posToHide.z);
        Vector2 b = new Vector2(transform.position.x, transform.position.z);
        float distance = Vector2.Distance(a, b);

        if (distance <= distanceToEat)
            ChangeState(OrcaState.WAITING);
        
    }

    public void StopEat()
    {
        ChangeState(OrcaState.HIDING);
    }

    private void OnDrawGizmosSelected()
    {
        if(PlayersManager.instance && ShipsManager.instance)
        {
            Gizmos.color = Color.cyan;
            Vector3 shipPos = new Vector3(ShipsManager.instance.playerShip.transform.position.x, 0, ShipsManager.instance.playerShip.transform.position.z);
            Gizmos.DrawWireSphere(shipPos, playerBoatRange);

            Gizmos.color = Color.green;
            foreach (PlayerController player in PlayersManager.instance.ingamePlayers)
            {
                Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                Gizmos.DrawLine(playerPos, shipPos);
            }
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanceToEat);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, orbitOffset);
    }

}
