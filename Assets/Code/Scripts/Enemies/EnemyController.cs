using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public EnemieManager enemieManager;
    public NavMeshAgent agent {  get; private set; }
    public Animator animator { get; private set; }

    public EnemyAction currentAction;
    private Rigidbody rb;

    [field: SerializeField]
    public ParticleSystem interactParticles {  get; private set; }

    [SerializeField]
    private float linkSpeed;
    private float baseSpeed;
    private float petrolSpeed;

    [Space, SerializedDictionary("Resource", "Mesh")]
    public SerializedDictionary<SteppedAction.ResourceType, GameObject> resourcesMeshes;

    [Space]
    [SerializeField] private LayerMask floorLayer;

    private bool inPetrol;

    public bool isFalseEnemy;


    private void Awake()
    {
        if (isFalseEnemy)
            return;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        foreach (KeyValuePair<SteppedAction.ResourceType, GameObject> item in resourcesMeshes)
            item.Value.SetActive(false);

        interactParticles.Stop(true);
    }

    private void Start()
    {
        if (isFalseEnemy)
            return;

        baseSpeed = agent.speed;
        petrolSpeed = agent.speed / 5;
    }

    void Update()
    {
        if(agent.enabled)
        {
            SetNavLinkSpeed();
            if (currentAction != null)
                currentAction.StateUpdate();
        }

        if(!rb.isKinematic && Physics.Raycast(transform.position, Vector3.down, 1.5f, floorLayer))
        {
            rb.isKinematic = true;
            agent.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Petrol"))
        {
            inPetrol = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Petrol"))
        {
            inPetrol = false;
        }
    }

    public void EnableResource(SteppedAction.ResourceType _resource, bool _enabled)
    {
        resourcesMeshes[_resource].SetActive(_enabled);
    }
    public void StopAction(bool _correctlyFinished)
    {
        foreach (KeyValuePair<SteppedAction.ResourceType, GameObject> item in resourcesMeshes)
            item.Value.SetActive(false);

        //Poner animacion de coger a false

        currentAction.onActionEnd -= StopAction;
        if(!_correctlyFinished)
            enemieManager.AddExistantAction(currentAction);
        currentAction = null;
    }

    private void SetNavLinkSpeed()
    {
        float currentSpeed = !inPetrol ? baseSpeed : petrolSpeed;
        agent.speed = !agent.isOnOffMeshLink ? currentSpeed : linkSpeed;
    }

    public void Knockback(Vector2 _force, Vector2 _direction)
    {
        agent.enabled = false;
        rb.isKinematic = false;
        Vector3 pushForward = _direction * _force.x;
        Vector3 pushUp = Vector3.up * _force.y;
        transform.position += Vector3.up * 0.5f;
        rb.AddForce((pushForward + pushUp), ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * 1.5f);
    }
}
