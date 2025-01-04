using UnityEngine;
public class FishingRod : Tool
{
    private bool fishingRodAdded = false;

    [HideInInspector]
    public bool isFishing;
    private bool hookThrowed;
    private bool hookLanded;
    [Space, Header("Hook"), SerializeField]
    private GameObject hookPrefab;
    [SerializeField]
    private Transform hookSpawnPoint;
    [SerializeField]
    private Vector2 throwForce;

    public HookController hook { get; private set; }

    public PlayerController player { get; private set; }
    public PlayerStateMachine playerSM { get; private set; }
    private void Start()
    {

        if (FishingManager.instance && !fishingRodAdded)
        {
            FishingManager.instance.AddFishingRod(this);
            fishingRodAdded = true;
        }

        isFishing = false;
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity).GetComponent<HookController>();
        hook.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            UseItem(null);
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        if (!isFishing && !hookThrowed) //Tirar anzuelo
            ThrowHook();
        else if (isFishing || hookLanded && !hook.onWater)//Recoger anzuelo
            GrabHook();
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);

        playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
        player = _objectHolder.GetComponentInParent<PlayerController>();
        playerSM.fishingState.fishingRod = this;
    }

    private void ThrowHook()
    {
        hook.gameObject.SetActive(true);
        hook.transform.position = hookSpawnPoint.position;
        hook.transform.rotation = hookSpawnPoint.rotation;

        hook.rb.linearVelocity = Vector3.zero;

        Vector3 throwDirection = hookSpawnPoint.forward * throwForce.x + Vector3.up * throwForce.y;
        hook.rb.AddForce(throwDirection, ForceMode.Impulse);

        playerSM.ChangeState(playerSM.fishingState);

        hookThrowed = true;
        Invoke("StartFishing", 0.75f);
    }
    private void StartFishing()
    {
        hookLanded = true;
        if (!hook.onWater)
            return;

        FishingManager.instance.FishingRodUsed(this);
        isFishing = true;
    }
    private void GrabHook()
    {
        FishingManager.instance.HookGrabbed(this);

        if (!hook.onWater)
        {
            playerSM.ChangeState(playerSM.idleState);
            isFishing = false;
        }
        hook.gameObject.SetActive(false);
        hookThrowed = false;
        hookLanded = false;

    }

    private void OnEnable()
    {
        if (FishingManager.instance)
        {
            FishingManager.instance.AddFishingRod(this);
            fishingRodAdded = true;
        }
    }
    private void OnDisable()
    {
        FishingManager.instance.RemoveFishingRod(this);
    }
}
