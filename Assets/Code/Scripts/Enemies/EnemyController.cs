using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent {  get; private set; }
    public Animator animator { get; private set; }

    public EnemyAction currentAction;

    [SerializedDictionary("Resource", "Mesh")]
    public SerializedDictionary<SteppedAction.ResourceType, GameObject> resourcesMeshes;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentAction != null)
            currentAction.StateUpdate();   
    }

    public void EnableResource(SteppedAction.ResourceType _resource, bool _enabled)
    {
        //resourcesMeshes[_resource].SetActive(_enabled);
    }
    public void StopAction()
    {
        currentAction.onActionEnd -= StopAction;
        currentAction = null;
    }
}
