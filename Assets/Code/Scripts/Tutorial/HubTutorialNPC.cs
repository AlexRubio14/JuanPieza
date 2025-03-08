using UnityEngine;
using UnityEngine.AI;

public class HubTutorialNPC : MonoBehaviour
{
    
    private NavMeshAgent agent;
    private Animator animator;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        
    }



}
