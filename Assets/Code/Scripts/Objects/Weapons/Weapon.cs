using System;
using System.Collections.Generic;
using UnityEngine;

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
    protected float minTiltProcess = 0.2f;
    protected bool shootReady;

    [Space, Header("Recoil"), SerializeField]
    protected Vector3 forwardAxis;
    [SerializeField]
    protected Vector2 recoilForce;
    [SerializeField]
    protected Vector2 recoilRotation;
    [Tooltip("0 pressets default (dejar vacio), 1 presets para el recoil"), SerializeField]
    protected RigidbodyRecoilPreset[] rbPresets;
    [SerializeField]
    protected float stopRecoilMagnitude;
    protected bool onRecoil;
    protected float recoilTimePassed;
    [SerializeField]
    protected float minRecoilDuration;

    [Serializable]
    public struct RigidbodyRecoilPreset
    {
        public float mass;
        public float drag;
        public float angularDrag;
    }

    [Space, Header("Particles"), SerializeField]
    protected GameObject shootParticles;
    [SerializeField]
    protected GameObject loadParticlesPrefab;
    protected List<ParticleSystem> loadParticles = new List<ParticleSystem>();

    [Space, Header("Weapon Audio"), SerializeField]
    protected AudioClip weaponShootClip;
    [SerializeField] protected AudioClip weaponReloadClip;
    [SerializeField] protected AudioClip shootReadyClip;
    [SerializeField] protected AudioClip shootStoppedClip;

    protected bool freeze;
    
    protected Animator animator;
    protected int mountedPlayerId = -1;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        hasAmmo = false;
        canUse = false;
    }

    protected override void Start()
    {
        base.Start();

        shootReady = false;
        tiltProcess = 0;
        tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);

        rbPresets[0].mass = rb.mass;
        rbPresets[0].drag = rb.linearDamping;
        rbPresets[0].angularDrag = rb.angularDamping;
    }
    protected void Update()
    {
        if (isTilting)
        {
            TiltWeapon();
            CheckIfReachedMaxTilt();
        }
        
        if (onRecoil)
            WaitRecoil();
    }

    public override void Grab(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken() || freeze || mountedPlayerId != -1)
            return;

        _objectHolder.ChangeObjectInHand(this ,false);
        rb.isKinematic = false;
        PlayerController player = _objectHolder.playerController;

        //Cambia el estado
        player.stateMachine.cannonState.SetWeapon(this);
        player.stateMachine.ChangeState(player.stateMachine.cannonState);
        isBeginUsed = true;
        mountedPlayerId = player.playerInput.playerReference;
        shootReady = false;
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        mountedPlayerId = -1;
        StopUse(_objectHolder);

        PlayerController player = _objectHolder.playerController;
        //Cambia el estado
        player.stateMachine.ChangeState(player.stateMachine.idleState);
        _objectHolder.RemoveItemFromHand();

        isBeginUsed = false;
        shootReady = false;
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (hasAmmo && handObject == this)
        {
            //Empezar a cargar el disparo
            isTilting = true;
            tiltProcess = 0;
            tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);
            PlayerController currentPlayer = _objectHolder.playerController;
            shootReady = false;
            currentPlayer.stateMachine.cannonState.SetWeapon(this);
            currentPlayer.stateMachine.ChangeState(currentPlayer.stateMachine.cannonState);
        }
    }
    public override void StopUse(ObjectHolder _objectHolder)
    {
        if (!isTilting)
            return;

        if (!shootReady)
        {
            //Hacer sonido 
            AudioManager.instance.Play2dOneShotSound(shootStoppedClip, "Objects", 1, 0.8f, 1.2f);
            //Resetear rotacion al 0
            tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);
            isTilting = false;
            return;
        }


        ShootWeapon();
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder))
            return;

        Reload(_objectHolder);
       
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        PlayerController playerCont = _objectHolder.playerController;

        return !state.GetIsBroken() && !freeze && !onRecoil //Si no esta roto ni congelado ni en medio del retroceso
            && !hasAmmo && handObject && handObject.objectSO == objectToInteract; //Si puede recargar
    }
    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        return base.CanGrab(_objectHolder) && !onRecoil && !isTilting && !freeze && mountedPlayerId == -1; 
    }
    public virtual void Reload(ObjectHolder _objectHolder)
    {
        //Settear a true la municion
        hasAmmo = true;
        canUse = true;
        //Borrar la bala de la mano
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        Destroy(currentObject.gameObject);

        animator.ResetTrigger("Shoot");
        animator.SetBool("HasAmmo", true);

        _objectHolder.playerController.animator.SetBool("Pick", false);

        foreach (ParticleSystem item in loadParticles)
            item.Play(true);

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

        float recoilTorqueForce = UnityEngine.Random.Range(recoilRotation.x, recoilRotation.y);
        float recoilTorqueDirection = GetClosestSign(UnityEngine.Random.Range(-1, 2));

        Vector3 recoilTorque = new Vector3(0, recoilTorqueForce * recoilTorqueDirection, 0);
        rb.AddTorque(recoilTorque, ForceMode.Impulse);
        onRecoil = true;
        recoilTimePassed = 0;
    }
    protected virtual void WaitRecoil()
    {
        recoilTimePassed += Time.deltaTime;
        if (recoilTimePassed < minRecoilDuration || rb.linearVelocity.magnitude > stopRecoilMagnitude)
            return;

        rb.linearVelocity = Vector3.zero;

        rb.mass = rbPresets[0].mass;
        rb.linearDamping = rbPresets[0].drag;
        rb.angularDamping = rbPresets[0].angularDrag;
        onRecoil = false;

    }
    protected int GetClosestSign(float _value)
    {
        return Mathf.Abs(1 - _value) < Mathf.Abs(-1 - _value) ? 1 : -1;
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
        canUse = false;
        animator.SetBool("HasAmmo", false);
        
        rb.constraints = RigidbodyConstraints.FreezeAll;

        UnMountPlayer();

    }
    protected override void RepairEnded()
    {
        base.RepairEnded();
        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    protected void ShootWeapon()
    {
        isTilting = false;
        hasAmmo = false;
        canUse = false;

        Shoot();

        animator.SetTrigger("Shoot");
        animator.SetBool("HasAmmo", false);

        if(mountedPlayerId != -1)
        {
            PlayerController controller = PlayersManager.instance.ingamePlayers[mountedPlayerId];
            controller.stateMachine.ChangeState(controller.stateMachine.idleState);
        }

        mountedPlayerId = -1;

        foreach (ParticleSystem item in loadParticles)
            item.Stop(true);

        tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);

        ApplyRecoil();
    }
    protected abstract void Shoot();

    public bool IsPlayerMounted()
    {
        return mountedPlayerId != -1;
    }
    public int GetMountedPlayerId()
    {
        return mountedPlayerId;
    }
    protected void TiltWeapon()
    {
        tiltProcess = Mathf.Clamp01(tiltProcess + tiltSpeed * Time.deltaTime);
        tiltObject.localRotation = Quaternion.Lerp(
            Quaternion.Euler(minWeaponTilt),
            Quaternion.Euler(maxWeaponTilt),
            tiltProcess
            );

        if(!shootReady && tiltProcess >= minTiltProcess)
        {
            shootReady = true;
            AudioManager.instance.Play2dOneShotSound(shootReadyClip, "Objects", 1, 0.8f, 1.2f);
        }

    }
    protected void CheckIfReachedMaxTilt()
    {
        if (tiltProcess < 1)
            return;

        ShootWeapon();
    }

    public void UnMountPlayer()
    {
        if (mountedPlayerId == -1)
        {
            isTilting = false;
            return;
        }

        PlayerController currentPlayer = PlayersManager.instance.ingamePlayers[mountedPlayerId];
        if (isTilting)
            currentPlayer.stateMachine.ChangeState(currentPlayer.stateMachine.idleState);

        isTilting = false;
        currentPlayer.animator.SetBool("Pick", false);
        currentPlayer.Release();
        mountedPlayerId = -1;
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
