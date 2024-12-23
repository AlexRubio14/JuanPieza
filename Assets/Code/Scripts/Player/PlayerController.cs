using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    [Header("Inputs"), SerializeField]
    private InputActionReference moveInputAction;
    [SerializeField]
    private InputActionReference rollInputAction;
    [SerializeField]
    private InputActionReference pushInputAction;

    [field: Space, Header("Movement"), SerializeField]
    public float baseMovementSpeed {  get; private set; }
    public Vector2 movementInput { get; private set; }
    public Vector3 movementDirection { get; private set; }

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine.InitializeStates(this);
    }

    private void Start()
    {
        canRoll = true;
    }

    private void OnEnable()
    {
        moveInputAction.action.started += MovementAction;
        moveInputAction.action.performed += MovementAction;
        moveInputAction.action.canceled += MovementAction;

        rollInputAction.action.started += RollAction;
        pushInputAction.action.started += PushAction;
    }
    private void OnDisable()
    {
        moveInputAction.action.started -= MovementAction;
        moveInputAction.action.performed -= MovementAction;
        moveInputAction.action.canceled -= MovementAction;

        rollInputAction.action.started -= RollAction;
        pushInputAction.action.started -= PushAction;
    }

    #region Input Actions 
    private void MovementAction(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>();
        movementDirection = new Vector3(movementInput.x, 0, movementInput.y);

    }
    private void RollAction(InputAction.CallbackContext obj)
    {
        if (canRoll)
        {
            stateMachine.currentState.RollAction();
            Invoke("WaitRollCD", rollCD);
        }
    }

    private void PushAction(InputAction.CallbackContext obj)
    {
        stateMachine.currentState.PushAction();
    }
    #endregion

    #region Actions
    public void Movement(Vector3 _direction, float _speed)
    {
        rb.AddForce(_direction * _speed, ForceMode.Force);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * pushOffset, pushRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisiona contra " + collision.contacts[0].otherCollider.gameObject.name + " | El estado es " + stateMachine.currentState.ToString());
        stateMachine.currentState.OnCollisionEnter(collision);
    }
}
