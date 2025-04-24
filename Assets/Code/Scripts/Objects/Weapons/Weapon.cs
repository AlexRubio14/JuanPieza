using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : RepairObject
{
    [Space, Header("Weapon"), SerializeField]
    protected Transform ridingPos;
    [field: SerializeField]
    public float rideOffset { get; protected set; }
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
    public bool isTilting {  get; protected set; }

    [Space, Header("Tilt Input Hint"), SerializeField]
    private TiltHintController tiltInputHint;

    [Space, Header("Recoil"), SerializeField]
    protected Vector3 forwardAxis;
    [SerializeField]
    protected Vector2 recoilForce;
    [SerializeField]
    protected float recoilRotation;
    [Serializable]
    public struct RigidbodyRecoilPreset
    {
        public float mass;
        public float drag;
        public float angularDrag;
    }
    [Tooltip("0 pressets default (dejar vacio), 1 presets para el recoil"), SerializeField]
    protected RigidbodyRecoilPreset[] rbPresets;
    [SerializeField]
    protected float stopRecoilMagnitude;
    protected bool onRecoil;
    private float recoilTimePassed;
    [Space, Header("Particles"), SerializeField]
    protected GameObject shootParticles;
    [SerializeField]
    protected GameObject loadParticlesPrefab;
    protected List<ParticleSystem> loadParticles = new List<ParticleSystem>();

    [Space, Header("Weapon Audio"), SerializeField]
    protected AudioClip weaponShootClip;
    [SerializeField] protected AudioClip weaponReloadClip;

    protected bool freeze;
    [HideInInspector]
    public bool isRotating;
    
    protected Animator animator;
    protected int mountedPlayerId = -1;

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

        rbPresets[0].mass = rb.mass;
        rbPresets[0].drag = rb.linearDamping;
        rbPresets[0].angularDrag = rb.angularDamping;
    }
    protected void Update()
    {
        if (isTilting)
            TiltWeapon();
        
        if (onRecoil)
            WaitRecoil();
    }

    public override void Grab(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken() || freeze)
            return;

        _objectHolder.ChangeObjectInHand(this ,false);
        rb.isKinematic = false;
        PlayerController player = _objectHolder.GetComponentInParent<PlayerController>();
        //Cambia el estado
        player.stateMachine.cannonState.SetWeapon(this);
        player.stateMachine.ChangeState(player.stateMachine.cannonState);
        isRotating = false;
        isBeginUsed = true;
        mountedPlayerId = player.playerInput.playerReference;
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        PlayerController player = _objectHolder.GetComponentInParent<PlayerController>();
        //Cambia el estado
        player.stateMachine.ChangeState(player.stateMachine.idleState);
        _objectHolder.RemoveItemFromHand();
        isBeginUsed = false;
        mountedPlayerId = -1;
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder))
            return;

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        InteractableObject nearestObj = _objectHolder.GetNearestInteractableObject();
        
        if(!hasAmmo && handObject && handObject.objectSO == objectToInteract)
            Reload(_objectHolder);
        else if (hasAmmo && !handObject)
        {
            //Empezar a cargar el disparo
            isTilting = true;
            tiltProcess = 0;
            tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);
            PlayerController currentPlayer = _objectHolder.GetComponentInParent<PlayerController>();
            mountedPlayerId = currentPlayer.playerInput.playerReference;
            currentPlayer.stateMachine.cannonState.SetWeapon(this);
            currentPlayer.stateMachine.ChangeState(currentPlayer.stateMachine.cannonState);
        }
    }
    public override void StopInteract(ObjectHolder _objectHolder)
    {
        if (!isTilting)
            return;

        isTilting = false;
        Shoot();
        animator.SetTrigger("Shoot");
        animator.SetBool("HasAmmo", false);
        PlayerController controller = PlayersManager.instance.ingamePlayers[mountedPlayerId];
        controller.stateMachine.ChangeState(controller.stateMachine.idleState);
        controller.animator.SetTrigger("Shoot");
        mountedPlayerId = -1;
        hint.interactType = HintController.ActionType.INTERACT;
        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);

        ApplyRecoil();
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        isRotating = !isRotating;
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();

        return !state.GetIsBroken() && !freeze && !onRecoil && (!hasAmmo && handObject && handObject.objectSO == objectToInteract || hasAmmo && !handObject);
    }
    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        return base.CanGrab(_objectHolder) && !onRecoil && !isTilting; 
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
    public virtual void Reload(ObjectHolder _objectHolder)
    {
        //Settear a true la municion
        hasAmmo = true;
        //Borrar la bala de la mano
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        Destroy(currentObject.gameObject);

        animator.ResetTrigger("Shoot");
        animator.SetBool("HasAmmo", true);

        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

        foreach (ParticleSystem item in loadParticles)
            item.Play(true);

        hint.interactType = HintController.ActionType.HOLD_INTERACT;

        AudioManager.instance.Play2dOneShotSound(weaponReloadClip, "Objects");
    }
    protected virtual void ApplyRecoil()
    {
        rb.mass = rbPresets[1].mass;
        rb.linearDamping = rbPresets[1].drag;
        rb.angularDamping = rbPresets[1].angularDrag;
        Vector3 currentAxis = transform.rotation * forwardAxis;
        Vector3 recoilToAdd = new Vector3(currentAxis.x * recoilForce.x, recoilForce.y, currentAxis.z * recoilForce.x);
        rb.AddForce(recoilToAdd , ForceMode.Impulse);

        Vector3 recoilTorque = new Vector3(0, UnityEngine.Random.Range(-recoilRotation, recoilRotation), 0);
        rb.AddTorque(recoilTorque, ForceMode.Impulse);
        onRecoil = true;
        recoilTimePassed = 0;
    }
    protected virtual void WaitRecoil()
    {
        recoilTimePassed += Time.deltaTime;
        if (recoilTimePassed < 1f || rb.linearVelocity.magnitude > stopRecoilMagnitude)
            return;

        rb.linearVelocity = Vector3.zero;

        rb.mass = rbPresets[0].mass;
        rb.linearDamping = rbPresets[0].drag;
        rb.angularDamping = rbPresets[0].angularDrag;
        onRecoil = false;

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

        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);

        hasAmmo = false;
        animator.SetBool("HasAmmo", false);
        
        rb.constraints = RigidbodyConstraints.FreezeAll;

        if (mountedPlayerId == -1)
        {
            isTilting = false;
            return;
        }

        PlayerController currentPlayer = PlayersManager.instance.ingamePlayers[mountedPlayerId];
        if(isTilting)
            currentPlayer.stateMachine.ChangeState(currentPlayer.stateMachine.idleState);
        
        isTilting = false;
        currentPlayer.animator.SetBool("Pick", false);
        currentPlayer.Release();
        mountedPlayerId = -1;

    }
    protected override void RepairEnded()
    {
        base.RepairEnded();
        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    protected abstract void Shoot();

    public bool IsPlayerMounted()
    {
        return mountedPlayerId != -1;
    }

    protected void TiltWeapon()
    {
        tiltProcess = Mathf.Clamp01(tiltProcess + tiltSpeed * Time.deltaTime);
        tiltObject.localRotation = Quaternion.Lerp(
            Quaternion.Euler(minWeaponTilt),
            Quaternion.Euler(maxWeaponTilt),
            tiltProcess
            );
    }
    protected void EnableTiltInputHint(bool _enable)
    {
        tiltInputHint.gameObject.SetActive(_enable);

        if (_enable)
            tiltInputHint.ChangeDeviceType(PlayersManager.instance.players[mountedPlayerId].Item1.devices[0] is Gamepad , PlayersManager.instance.ingamePlayers[mountedPlayerId].transform);
    }

    public void SetFreeze(bool _freeze)
    {
        freeze = _freeze;
    }

    public bool GetFreeze()
    {
        return freeze;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 endPos = transform.position + transform.rotation * forwardAxis;
        Gizmos.DrawLine(transform.position, endPos);
    }
}
