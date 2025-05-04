using UnityEngine;

public class RescueNPC : MonoBehaviour
{
    [HideInInspector]
    public bool rescued {  get; private set; }

    [HideInInspector]
    public bool isSwimming;
    public GameObject hookToFollow{ get; private set; }
    private Vector3 endPosition;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        isSwimming = false;

        animator.SetTrigger("Dead");
        animator.SetBool("Swimming", true);

        transform.SetParent(null);

        FishingManager.instance.AddTutorialNPC(this);
    }

    void Update()
    {

        if (hookToFollow && !hookToFollow.activeInHierarchy)
            hookToFollow = null;

        if (!rescued && hookToFollow)
        {
            //Si la distancia hacia el  anzuelo es menor a X esperar a ser rescatado
            if (Vector3.Distance(transform.position, endPosition) <= 1)
                WaitToGetRescued();
            else
                SwimToHook();
        }else
        {
            animator.SetBool("Moving", false);
        }

        animator.SetBool("Moving", isSwimming);
    }
    private void WaitToGetRescued()
    {
        isSwimming = false;
    }
    private void SwimToHook()
    {
        //Lerp de la posicion actual hacia la del anzuelo mas cercano
        transform.position = transform.position + (endPosition - transform.position).normalized * 2.3f * Time.deltaTime;
        transform.forward = (endPosition - transform.position).normalized;
        isSwimming = true;
        animator.SetBool("Moving", true);
    }
    public void HookRemoved()
    {
        hookToFollow = null;
        isSwimming = false;
    }
    public void SetHookPosition(GameObject _hook)
    {
        hookToFollow = _hook;
        Vector3 finalHookPos = hookToFollow.transform.position;
        finalHookPos.y = FishingManager.instance.defaultYPos;
        endPosition = finalHookPos;
    }

    public void NPCRescued()
    {
        transform.forward = (new Vector3(transform.position.x, 0, transform.position.z) -  new Vector3(endPosition.x, 0, endPosition.z)).normalized;
        rescued = true;
        isSwimming = false;
        hookToFollow = null;
        animator.SetBool("Swimming", false);
        transform.SetParent(ShipsManager.instance.playerShip.transform);

        RescueManager.instance.RemoveNpcCount();
    }
    
}
