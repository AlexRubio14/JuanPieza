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

    [SerializeField]
    private float linkSpeed;
    private float baseSpeed;

    [Space, SerializedDictionary("Resource", "Mesh")]
    public SerializedDictionary<SteppedAction.ResourceType, GameObject> resourcesMeshes;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        foreach (KeyValuePair<SteppedAction.ResourceType, GameObject> item in resourcesMeshes)
            item.Value.SetActive(false);
    }

    private void Start()
    {
        baseSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        SetNavLinkSpeed();
        
        if (currentAction != null)
            currentAction.StateUpdate();

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
        agent.speed = !agent.isOnOffMeshLink ? baseSpeed : linkSpeed;
    }
}
