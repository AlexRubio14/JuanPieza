using UnityEngine;

public class TutorialSwimNPC : MonoBehaviour
{
    [SerializeField]
    private DialogueData rescuedDialogue;

    [HideInInspector]
    public bool rescued {  get; private set; }

    [HideInInspector]
    public bool isSwimming;
    public Vector3 hookPosition { get; private set; }
    private Vector3 endPosition;

    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        isSwimming = false;

        hookPosition = Vector3.zero;
        FishingManager.instance.AddTutorialNPC(this);

        animator.SetTrigger("Dead");
        animator.SetBool("Swimming", true);

        transform.SetParent(null);

    }

    void Update()
    {
        if (!rescued && hookPosition != Vector3.zero)
        {
            //Si la distancia hacia el  anzuelo es menor a X esperar a ser rescatado
            if (Vector3.Distance(transform.position, endPosition) <= 1)
                WaitToGetRescued();
            else
                SwimToHook();
        }
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
    }
    public void HookRemoved()
    {
        hookPosition = Vector3.zero;
        isSwimming = false;
    }
    public void SetHookPosition(Vector3 _hookPos)
    {
        Vector3 finalHookPos = _hookPos;
        finalHookPos.y = FishingManager.instance.defaultYPos;
        hookPosition = finalHookPos;

        endPosition = hookPosition;
    }

    public void NPCRescued()
    {
        transform.forward = (new Vector3(transform.position.x, 0, transform.position.z) -  new Vector3(endPosition.x, 0, endPosition.z)).normalized;
        rescued = true;
        isSwimming = false;
        hookPosition = Vector3.zero;
        animator.SetBool("Swimming", false);
        transform.SetParent(ShipsManager.instance.playerShip.transform);

        DialogueController.instance.StartDialogue(rescuedDialogue);
    }
    
}
