using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform destiny;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.SetDestination(destiny.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
