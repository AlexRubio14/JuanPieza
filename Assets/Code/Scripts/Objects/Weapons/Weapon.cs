using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : RepairObject
{
    [Space, Header("Weapon"), SerializeField]
    protected Transform ridingPos;

    [field: SerializeField] 
    public float coolDown = 2;

    [Space, Header("Damage"), SerializeField]
    protected float weaponDamage;

    [field: SerializeField]
    public float bulletForce { get; protected set; }
    public bool hasAmmo { get; protected set; }

    [field: Space,Header("Tilt"), SerializeField]
    public Transform tiltObject { get; protected set; }
    [field: SerializeField]
    public Vector3 minWeaponTilt { get; protected set; }
    [field: SerializeField]
    public Vector3 maxWeaponTilt { get; protected set; }
    [field: SerializeField]
    public float tiltSpeed { get; protected set; }

    [HideInInspector]
    public float tiltProcess;

    [Header("Tilt Input Hint"), SerializeField]
    private TiltHintController tiltInputHint;


    protected Animator animator;
    protected int mountedPlayerId = -1;

    [Space, Header("Particles"), SerializeField]
    protected GameObject shootParticles;
    [SerializeField]
    protected GameObject loadParticlesPrefab;
    protected List<ParticleSystem> loadParticles = new List<ParticleSystem>();

    [Space, Header("Weapon Audio"), SerializeField]
    protected AudioClip weaponShootClip;
    [SerializeField] protected AudioClip weaponReloadClip;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        hasAmmo = false;
    }

    protected override void Start()
    {
        base.Start();
        tiltProcess = 0;
        tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);

        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        PlayerController player = _objectHolder.transform.parent.gameObject.GetComponent<PlayerController>();
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (isPlayerMounted() && player.playerInput.playerReference == mountedPlayerId) //Desmontarse
            UnMount(player, _objectHolder);
        else if (!isPlayerMounted()) //Montarse al arma
            Mount(player, _objectHolder);
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        if (!hasAmmo)
            return;

        Shoot();            
        animator.SetTrigger("Shoot");
        animator.SetBool("HasAmmo", false);

        foreach (ParticleSystem item in loadParticles)
        {
            item.Stop(true);
        }
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
        {
            return base.CanInteract(_objectHolder);
        }

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();

        return !isPlayerMounted() && !handObject /*Montarse*/ || isPlayerMounted() && playerCont.playerInput.playerReference == mountedPlayerId /*Bajarse*/ || !hasAmmo && handObject && handObject.objectSO == objectToInteract /*Recargar*/ ;
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        
        if (!hasAmmo)
            tooltip.SetState(ObjectsTooltip.ObjectState.Empty);
        else
            tooltip.SetState(ObjectsTooltip.ObjectState.Loaded);
        
        if (!handObject && !isPlayerMounted()) //No tiene nada en la mano y no hay nadie montado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "mount"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (isPlayerMounted() && !hasAmmo && playerCont.playerInput.playerReference == mountedPlayerId) //Si lo esta utilizando y no esta cargado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "dismount"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (!hasAmmo && handObject && handObject.objectSO == objectToInteract)//Si no esta cargado y tiene la bala en la mano
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "load_bullet"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (hasAmmo && playerCont.playerInput.playerReference == mountedPlayerId) //Si esta cargado y el player esta montado
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "dismount"),
                new HintController.Hint(HintController.ActionType.USE, "shoot")
            };        

        return new HintController.Hint[] 
        {
            new HintController.Hint(HintController.ActionType.NONE, "") 
        };
    }

    protected void Mount(PlayerController _player, ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this, false);

        mountedPlayerId = _player.playerInput.playerReference;
        //Cambiar el mapa de inputs
        PlayersManager.instance.players[mountedPlayerId].Item1.SwitchCurrentActionMap("CannonGameplay");
        //Cambiar estado del player
        PlayerStateMachine playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
        playerSM.cannonState.SetWeapon(this);
        playerSM.ChangeState(playerSM.cannonState);
        //Hacer Tp al player a la posicion de montar al ca�on
        _player.transform.position = ridingPos.position;
        _player.transform.forward = ridingPos.forward;
        //El padre del arma sera el player 
        transform.SetParent(_player.transform);

        selectedVisual.Hide();


        EnableTiltInputHint(true);

    }
    protected void UnMount(PlayerController _player, ObjectHolder _objectHolder)
    {
        //Cambiar el mapa de inputs
        PlayersManager.instance.players[_player.playerInput.playerReference].Item1.SwitchCurrentActionMap("Gameplay");

        EnableTiltInputHint(false);

        //Cambiar estado del player
        PlayerStateMachine playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
        playerSM.ChangeState(playerSM.idleState);

        //Quitar el ca�on del player
        _objectHolder.RemoveItemFromHand();

        mountedPlayerId = -1;
    }
    public void Reload(ObjectHolder _objectHolder)
    {
        //Settear a true la municion
        hasAmmo = true;
        //Borrar la bala de la mano
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        Destroy(currentObject.gameObject);

        animator.SetBool("HasAmmo", true);

        foreach (ParticleSystem item in loadParticles)
            item.Play(true);

        AudioManager.instance.Play2dOneShotSound(weaponReloadClip, "Objects");
    }

    protected void AddLoadParticle(Transform _parent)
    {
        ParticleSystem loadParticleSystem = Instantiate(loadParticlesPrefab, _parent.position, Quaternion.identity).GetComponent<ParticleSystem>();
        loadParticleSystem.transform.SetParent(_parent);
        loadParticleSystem.transform.position = _parent.position;
        loadParticleSystem.transform.forward = _parent.forward;
        loadParticles.Add(loadParticleSystem);
        loadParticleSystem.Stop(true);
    }

    public override void OnBreakObject()
    {
        base.OnBreakObject();

        hasAmmo = false;
        animator.SetBool("HasAmmo", false);
        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);
        
        if (mountedPlayerId == -1)
            return;

        PlayerController currentPlayer = PlayersManager.instance.ingamePlayers[mountedPlayerId];
        UnMount(currentPlayer, currentPlayer.objectHolder);
        currentPlayer.animator.SetBool("Pick", false);
        
    }
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        base.RepairEnded(_objectHolder);
        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);
    }
    protected abstract void Shoot();

    public bool isPlayerMounted()
    {
        return mountedPlayerId != -1;
    }

    protected void EnableTiltInputHint(bool _enable)
    {
        tiltInputHint.gameObject.SetActive(_enable);

        if (_enable)
            tiltInputHint.ChangeDeviceType(PlayersManager.instance.players[mountedPlayerId].Item1.devices[0] is Gamepad , PlayersManager.instance.ingamePlayers[mountedPlayerId].transform);
    }
}
