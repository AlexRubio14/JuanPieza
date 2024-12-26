using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

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
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float slopeOffset;
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

    [field: Space, Header("Push"), SerializeField]
    public float pushRadius { get; private set; }
    [field: SerializeField]
    public float pushOffset { get; private set; }
    [field: SerializeField]
    public LayerMask pushLayers { get; private set; }
    [field: SerializeField]
    public Vector2 pushForce { get; private set; }

    private Rigidbody rb;

    public GameObject item { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine.InitializeStates(this);
    }

    private void Start()
    {
        SuscribeActions();
        canRoll = true;
    }

    private void OnEnable()
    {
        SuscribeActions();
    }
    private void SuscribeActions()
    {
        if (!playerInput)
            return;

        playerInput.OnMoveAction += MovementAction;

        playerInput.OnRollAction += RollAction;

        playerInput.OnInteractAction += InteractAction;

        playerInput.OnUseAction += UseAction;
    }

    private void OnDisable()
    {
        playerInput.OnMoveAction -= MovementAction;

        playerInput.OnRollAction -= RollAction;

        playerInput.OnInteractAction -= InteractAction;

        playerInput.OnUseAction -= UseAction;
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

    private void UseAction()
    {
        stateMachine.currentState.UseAction();
    }
    #endregion

    #region Actions
    public void Movement(Vector3 _direction, float _speed)
    {
        rb.AddForce(_direction * _speed, ForceMode.Force);
    }
    public void CheckSlope()
    {
        //Hacer Raycast en las tres direcciones
        foreach (Transform slopePosition in slopePositions)
        {
            if (!Physics.Raycast(slopePosition.position, slopePosition.forward, slopeCheckDistance, slopeCheckLayer))
                continue;
            
            //Si choca 
            //Hacer otro raycast mas arriba
            for (float i = slopeOffset; i < maxSlopeHeight; i += slopeOffset)
            {
                Vector3 currentSlopePos = slopePosition.position;
                currentSlopePos.y += i;

                //Hacer otro raycast
                if (!Physics.Raycast(currentSlopePos, slopePosition.forward, slopeCheckDistance, slopeCheckLayer))
                {
                    rb.position = rb.position + new Vector3(0, i + 0.25f, 0);
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
    #endregion


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

    public void SetItem(GameObject _item)
    {
        item = _item;
    }
}
