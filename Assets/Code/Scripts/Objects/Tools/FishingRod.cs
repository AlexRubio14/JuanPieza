using UnityEngine;
public class FishingRod : Tool, ICatapultAmmo
{
    private bool fishingRodAdded = false;

    [HideInInspector]
    public bool isFishing;
    private bool hookThrowed;
    private bool hookLanded;
    private bool hookGrabbed;
    [Space, Header("Fishing Rod"), SerializeField]
    private GameObject idleFishingRod;
    [SerializeField]
    private GameObject landedFishingRod;

    [Space, Header("Hook"), SerializeField]
    private GameObject hookPrefab;
    public bool chargingHook {  get; private set; }
    private float tiltProcess; 
    [SerializeField]
    private Transform hookSpawnPoint;
    [SerializeField]
    private float tiltSpeed;
    [SerializeField]
    private Vector3 minTilt;
    [SerializeField]
    private Vector3 maxTilt;
    [field: SerializeField]
    public Vector2 throwForce { get; private set; }



    public HookController hook { get; private set; }

    public PlayerController player { get; private set; }
    protected virtual void Start()
    {

        if (FishingManager.instance && !fishingRodAdded)
        {
            FishingManager.instance.AddFishingRod(this);
            fishingRodAdded = true;
        }

        chargingHook = false;
        isFishing = false;
        hook = Instantiate(hookPrefab, transform.position, Quaternion.identity).GetComponent<HookController>();
        hook.gameObject.SetActive(false);

        hookSpawnPoint.localRotation = Quaternion.Euler(minTilt);
        tiltProcess = 0;
    }
    private void Update()
    {
        if (chargingHook)
            ChargeHook();
    }
    protected override void OnEnable()
    {
        ShipsManager.instance.playerShip.AddInteractuableObject(this);

        if (FishingManager.instance && !fishingRodAdded)
        {
            FishingManager.instance.AddFishingRod(this);
            fishingRodAdded = true;
        }
    }
    private void OnDisable()
    {
        FishingManager.instance.RemoveFishingRod(this);
    }

    public override void Grab(ObjectHolder _objectHolder)
    {

        FishingManager.instance.ResetFishingRodData(this);

        PlayerController currentPlayer = _objectHolder.GetComponentInParent<PlayerController>();
        currentPlayer.animator.ResetTrigger("FishingStop");
        hookGrabbed = false;
        hint.useType = HintController.ActionType.HOLD_USE;
        base.Grab(_objectHolder);

        if (player == currentPlayer)
            return;

        player = currentPlayer;
        player.stateMachine.fishingState.fishingRod = this;
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        chargingHook = false;
        hookGrabbed = false;
        isFishing = false;
        hookThrowed = false;
        GrabHook();
        idleFishingRod.SetActive(true);
        landedFishingRod.SetActive(false);
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        playerCont.stateMachine.ChangeState(playerCont.stateMachine.idleState);
        playerCont.animator.SetBool("FishingCharge", false);
        base.Release(_objectHolder);
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        if (!isFishing && !hookThrowed) //Cargar el anzuelo
        {
            chargingHook = true;
            tiltProcess = 0;
            if (!player)
                player = _objectHolder.GetComponentInParent<PlayerController>();


            player.stateMachine.fishingState.fishingRod = this;
            player.animator.SetBool("FishingCharge", true);
            player.stateMachine.ChangeState(player.stateMachine.fishingState);
        }
        else if (isFishing || hookLanded && !hook.onWater)//Recoger anzuelo
        {
            hookGrabbed = true;
            GrabHook();
        }
    }
    public override void StopUse(ObjectHolder _objectHolder)
    {
        if(hookGrabbed)
            hookGrabbed = false;
        else if(!isFishing && !hookThrowed)
        {
            chargingHook = false;
            ThrowHook(_objectHolder);
        }

    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Interact(_objectHolder);
    }


    private void ChargeHook()
    {
        tiltProcess = Mathf.Clamp01(tiltProcess + tiltSpeed * Time.deltaTime);
        //Debug.Break();

        hookSpawnPoint.localRotation = Quaternion.Lerp(
            Quaternion.Euler(minTilt),
            Quaternion.Euler(maxTilt),
            tiltProcess
            );
    }
    private void ThrowHook(ObjectHolder _objectHolder)
    {
        PlayerController currentPlayer = _objectHolder.GetComponentInParent<PlayerController>();
            
        if (!player || player != currentPlayer)
        {

            player = currentPlayer;
            player.stateMachine.fishingState.fishingRod = this;
        }
        hook.gameObject.SetActive(true);
        hook.transform.position = hookSpawnPoint.position;
        hook.transform.rotation = _objectHolder.transform.rotation;

        hook.rb.linearVelocity = Vector3.zero;

        Vector3 throwDirection = hookSpawnPoint.forward * throwForce.x + Vector3.up * throwForce.y;

        hook.rb.AddForce(throwDirection, ForceMode.Impulse);

        hookThrowed = true;

        idleFishingRod.SetActive(false);
        landedFishingRod.SetActive(true);

        player.animator.SetBool("FishingCharge", false);

        hint.useType = HintController.ActionType.USE;

        Invoke("StartFishing", 0.75f);
    }
    private void StartFishing()
    {
        if (!hookThrowed)
            return;

        hookLanded = true;
        if (!hook.onWater)
            return;

        FishingManager.instance.FishingRodUsed(this);
        isFishing = true;
    }
    public void GrabHook()
    {

        StartCoroutine(FishingManager.instance.HookGrabbed(this));

        if (!hook.onWater)
        {
            if(player)
                player.stateMachine.ChangeState(player.stateMachine.idleState);
            isFishing = false;
        }
        hook.gameObject.SetActive(false);
        hookThrowed = false;
        hookLanded = false;

        idleFishingRod.SetActive(true);
        landedFishingRod.SetActive(false);

        hint.useType = HintController.ActionType.HOLD_USE;

        if (player)
            player.animator.SetTrigger("FishingStop");
    }

    
}
