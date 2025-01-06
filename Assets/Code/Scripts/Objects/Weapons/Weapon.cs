using UnityEngine;

public abstract class Weapon : InteractableObject
{
    [Space, Header("Weapon"), SerializeField]
    protected Transform ridingPos;

    [field: SerializeField]
    public float bulletForce {  get; protected set; }
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

    protected Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        hasAmmo = false;
    }

    private void Start()
    {
        tiltProcess = 0;
        tiltObject.localRotation = Quaternion.Euler(minWeaponTilt);
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder))
            return;

        PlayerController player = _objectHolder.transform.parent.gameObject.GetComponent<PlayerController>();
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (isBeingUsed) //Desmontarse
            UnMount(player, _objectHolder);
        else if (!hasAmmo && handObject && handObject.objectSO == objectToInteract) //Si no tiene municion agregar la bala al cañon
            Reload(_objectHolder);
        else if (!handObject) //Montarse al arma
            Mount(player, _objectHolder);
    }
    public override void UseItem(ObjectHolder _objectHolder)
    {
        if(hasAmmo)
            Shoot();
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject /*Montarse*/ || isBeingUsed /*Bajarse*/ || !hasAmmo && handObject && handObject.objectSO == objectToInteract /*Recargar*/ ;
    }

    protected void Mount(PlayerController _player, ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this, false);

        //Cambiar el mapa de inputs
        PlayersManager.instance.players[_player.playerInput.playerReference].Item1.SwitchCurrentActionMap("CannonGameplay");
        //Cambiar estado del player
        PlayerStateMachine playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
        playerSM.cannonState.SetWeapon(this);
        playerSM.ChangeState(playerSM.cannonState);
        //Hacer Tp al player a la posicion de montar al cañon
        _player.transform.position = ridingPos.position;
        _player.transform.forward = ridingPos.forward;
        //El padre del arma sera el player 
        transform.SetParent(_player.transform);

        isBeingUsed = true;
        selectedVisual.Hide();
    }
    protected void UnMount(PlayerController _player, ObjectHolder _objectHolder)
    {
        //Cambiar el mapa de inputs
        PlayersManager.instance.players[_player.playerInput.playerReference].Item1.SwitchCurrentActionMap("Gameplay");

        //Cambiar estado del player
        PlayerStateMachine playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
        playerSM.ChangeState(playerSM.idleState);

        //Quitar el cañon del player
        _objectHolder.RemoveItemFromHand();
    }
    protected void Reload(ObjectHolder _objectHolder)
    {
        //Settear a true la municion
        hasAmmo = true;
        //Borrar la bala de la mano
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        Destroy(currentObject.gameObject);
    }

    protected abstract void Shoot();
}
