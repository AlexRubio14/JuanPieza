using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStateMachine stateMachine {  get; private set; }

    [HideInInspector]
    public GameInput playerInput;

    [field: Space, Header("Movement"), SerializeField]
    public float baseMovementSpeed {  get; private set; }
    public Vector2 movementInput { get; private set; }
    public Vector3 movementDirection { get; private set; }

    [field: Space, Header("Rotation"), SerializeField]
    public float rotationSpeed {  get; private set; }

    [Space, Header("Slope"), SerializeField]
    private Transform[] slopePositions;
    [SerializeField]
    public float maxSlopeHeight;
    [field: SerializeField]
    public float slopeCheckDistance { get; private set; }
    [field: SerializeField]
    public float slopeOffset {  get; private set; }
    [SerializeField]
    private LayerMask slopeCheckLayer;

    [field: Space, Header("Roll"), SerializeField]
    public float rollSpeed { get; private set; }
    [field: SerializeField]
    public float rollDuration { get; private set; }
    [SerializeField]
    private float rollCD;
    private bool canRoll;
    [field: SerializeField]
    public Vector2 bounceForce { get; private set; }
    [field: SerializeField]
    public float rollSlopeDistance {  get; private set; }
    [field: SerializeField]
    public float rollSlopeOffset {  get; private set; }

    [field: Space, Header("Push"), SerializeField]
    public float pushRadius { get; private set; }
    [field: SerializeField]
    public float pushOffset { get; private set; }
    [field: SerializeField]
    public LayerMask pushLayers { get; private set; }
    [field: SerializeField]
    public Vector2 pushForce { get; private set; }

    public Rigidbody rb { get; private set; }

    [SerializeField] public ObjectHolder objectHolder;


    [field: Space, Header("Death"), SerializeField]
    public float timeToDie {  get; private set; }
    [field: SerializeField]
    public float swimSpeed { get; private set; }

    [Space, Header("Interact"), SerializeField]
    private Canvas interactCanvas;

    public GameObject interactCanvasObject => interactCanvas.transform.gameObject;


    [Header("Votation")]
    public bool votationDone {  get; set; }

    [field: Space, Header("Cannon"), SerializeField]
    public float cannonSpeed { get; private set; }
    [field:SerializeField]
    public float cannonRotationSpeed { get; private set; }
    public float cannonTilt {  get; private set; }


    public Animator animator { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine.InitializeStates(this);
    }

    private void Start()
    {
        SuscribeActions();
        canRoll = true;
        objectHolder = GetComponentInChildren<ObjectHolder>();
        interactCanvas.worldCamera = Camera.main;
        interactCanvasObject.SetActive(false);
    }

    private void OnEnable()
    {
        SuscribeActions();
        PlayersManager.instance.ingamePlayers.Add(this);
    }
    private void SuscribeActions()
    {
        if (!playerInput)
            return;

        playerInput.OnMoveAction += MovementAction;

        playerInput.OnRollAction += RollAction;

        playerInput.OnInteractAction += InteractAction;

        playerInput.OnStopInteractAction += StopInteractAction;

        playerInput.OnUseAction += UseAction;

        playerInput.OnWeaponMoveAction += CannonMovementAction;

        playerInput.OnWeaponTiltAction += CannonTiltAction;
    }

    private void OnDisable()
    {
        playerInput.OnMoveAction -= MovementAction;

        playerInput.OnRollAction -= RollAction;

        playerInput.OnInteractAction -= InteractAction;

        playerInput.OnStopInteractAction -= StopInteractAction;

        playerInput.OnUseAction -= UseAction;

        playerInput.OnWeaponMoveAction -= CannonMovementAction;

        playerInput.OnWeaponTiltAction -= CannonTiltAction;

        PlayersManager.instance.ingamePlayers.Remove(this);
    }

    #region Input Actions 
    private void MovementAction(Vector2 _movementInput)
    {
        movementInput = _movementInput;
        movementDirection = new Vector3(movementInput.x, 0, movementInput.y);
    }
    private void RollAction()
    {
        if (canRoll)
        {
            stateMachine.currentState.RollAction();
            Invoke("WaitRollCD", rollCD);
        }
    }
    private void InteractAction()
    {
        stateMachine.currentState.InteractAction();        
    }
    private void StopInteractAction()
    {
        stateMachine.currentState.StopInteractAction();
    }
    private void UseAction()
    {
        stateMachine.currentState.UseAction();        
    }

    private void CannonMovementAction(bool _isForward, float _axis)
    {
        Vector2 newInput = movementInput;
        if(_isForward)
            newInput.x = _axis;
        else
            newInput.y = _axis;

        movementInput = newInput;
    }
    private void CannonTiltAction(float _axis)
    {
        cannonTilt = _axis;
    }
    #endregion

    #region Actions
    public void Movement(Vector3 _direction, float _speed)
    {
        rb.AddForce(_direction * _speed, ForceMode.Force);
    }
    public void CheckSlope(float _slopeLength, float _slopeOffset)
    {
        //Hacer Raycast en las tres direcciones
        foreach (Transform slopePosition in slopePositions)
        {
            RaycastHit hit;
            Physics.Raycast(slopePosition.position, slopePosition.forward, out hit, _slopeLength, slopeCheckLayer);
            if (!hit.collider)
                continue;
            
            //Si choca 
            //Hacer otro raycast mas arriba
            for (float i = _slopeOffset; i < maxSlopeHeight; i += _slopeOffset)
            {
                Vector3 currentSlopePos = slopePosition.position;
                currentSlopePos.y += i;

                //Hacer otro raycast
                if (!Physics.Raycast(currentSlopePos, slopePosition.forward, _slopeLength, slopeCheckLayer))
                {   
                    rb.position = hit.point + new Vector3(0, i + 0.25f, 0);
                    //Debug.Break();
                    return;
                }
            }
        }
       
        
    }
    public void Rotate(Vector3 _desiredRotation, float _speed)
    {
        Vector3 finalRotation = Vector3.Slerp(transform.forward, _desiredRotation, _speed * Time.fixedDeltaTime);
        transform.forward = finalRotation;
    }
    public void SetRotation(Vector3 _desiredRotation)
    {
        transform.forward = _desiredRotation;
    }
    public void AddImpulse(Vector3 _direction, float _force)
    {
        rb.AddForce(_direction * _force, ForceMode.Impulse);
    }
    private void WaitRollCD()
    {
        canRoll = true;
    }
    public void Interact()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();
        if (!objectHolder.GetHandInteractableObject() && 
            (!objectHolder.GetNearestInteractableObject() || 
            objectHolder.GetNearestInteractableObject() && !objectHolder.GetNearestInteractableObject().CanInteract(objectHolder))
            )
            return;

        if (objectHolder.GetNearestInteractableObject() && objectHolder.GetNearestInteractableObject().CanInteract(objectHolder))
            objectHolder.GetNearestInteractableObject().Interact(objectHolder);
        else
            objectHolder.GetHandInteractableObject().Interact(objectHolder);

        animator.SetBool("Pick", objectHolder.GetHandInteractableObject());

    }
    public void StopInteract()
    {
        InteractableObject nearObject = objectHolder.GetNearestInteractableObject();

        if (nearObject as Repair)
        {
            if (((Repair)nearObject).IsPlayerReparing(this))
                nearObject.StopInteract(objectHolder);

        }
    }
    public void Use()
    {
        if(objectHolder.GetHandInteractableObject())
            objectHolder.GetHandInteractableObject().UseItem(objectHolder);
    }
    #endregion

    public Rigidbody GetRB() { return rb; }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colisiona contra " + collision.contacts[0].otherCollider.gameObject.name + " | El estado es " + stateMachine.currentState.ToString());
        stateMachine.currentState.OnCollisionEnter(collision);

    }

    private void OnDrawGizmos()
    {
        //Gizmo del area de empujar
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.forward * pushOffset, pushRadius);

        //Gizmos del forward del personaje
        Gizmos.color = Color.blue;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * 2;
        Gizmos.DrawLine(startPos, endPos);


        //Gizmos de los raycast que hace para subir escalones
        foreach (Transform slopePosition in slopePositions)
        {
            Gizmos.color = Color.red;
            startPos = slopePosition.position;
            endPos = startPos + slopePosition.forward * slopeCheckDistance;
            Gizmos.DrawLine(startPos, endPos);

            Gizmos.color = Color.yellow;
            for (float i = slopeOffset; i < maxSlopeHeight; i += slopeOffset)
            {
                startPos = slopePosition.position + new Vector3(0, i, 0);
                endPos = startPos + slopePosition.forward * slopeCheckDistance;
                Gizmos.DrawLine(startPos, endPos);
            }

            Gizmos.color = Color.green;
            startPos = slopePosition.position + new Vector3(0, maxSlopeHeight, 0);
            endPos = startPos + slopePosition.forward * slopeCheckDistance;
            Gizmos.DrawLine(startPos, endPos);
        }
    }

}
