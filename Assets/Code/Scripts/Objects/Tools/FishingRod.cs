using UnityEngine;
public class FishingRod : Tool
{
    private bool fishingRodAdded = false;

    [HideInInspector]
    public bool isFishing;
    private bool hookThrowed;
    private bool hookLanded;

    [Space, Header("Fishing Rod"), SerializeField]
    private GameObject idleFishingRod;
    [SerializeField]
    private GameObject landedFishingRod;

    [Space, Header("Hook"), SerializeField]
    private GameObject hookPrefab;
    [SerializeField]
    private Transform hookSpawnPoint;
    [SerializeField]
    private Vector2 throwForce;

    public HookController hook { get; private set; }

    public PlayerController player { get; private set; }
    public PlayerStateMachine playerSM { get; private set; }
    protected override void Start()
    {
        base.Start();

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
            ThrowHook(_objectHolder);
        else if (isFishing || hookLanded && !hook.onWater)//Recoger anzuelo
            GrabHook();
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
        if (!player)
        {
            playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
            player = _objectHolder.GetComponentInParent<PlayerController>();
            playerSM.fishingState.fishingRod = this;
        }
    }

    private void ThrowHook(ObjectHolder _objectHolder)
    {
        if (!player)
        {
            playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
            player = _objectHolder.GetComponentInParent<PlayerController>();
            playerSM.fishingState.fishingRod = this;
        }
        hook.gameObject.SetActive(true);
        hook.transform.position = hookSpawnPoint.position;
        hook.transform.rotation = _objectHolder.transform.rotation;

        hook.rb.linearVelocity = Vector3.zero;

        Vector3 throwDirection = hookSpawnPoint.forward * throwForce.x + Vector3.up * throwForce.y;
        hook.rb.AddForce(throwDirection, ForceMode.Impulse);

        playerSM.ChangeState(playerSM.fishingState);

        hookThrowed = true;

        idleFishingRod.SetActive(false);
        landedFishingRod.SetActive(true);

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

        idleFishingRod.SetActive(true);
        landedFishingRod.SetActive(false);

    }

    protected override void OnEnable()
    {
        base.OnEnable();
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
