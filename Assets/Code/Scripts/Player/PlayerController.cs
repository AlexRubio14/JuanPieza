using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    [Header("Inputs"), SerializeField]
    private InputActionReference moveInputAction;
    [SerializeField]
    private InputActionReference rollInputAction;
    [SerializeField]
    private InputActionReference pushInputAction;
    [SerializeField]
    private InputActionReference interactInputAction;
    [SerializeField]
    private InputActionReference useInputAction;

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

    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] public ObjectHolder objectHolder;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine.InitializeStates(this);
    }

    private void Start()
    {
        canRoll = true;
        objectHolder = GetComponentInChildren<ObjectHolder>();
    }

    private void OnEnable()
    {
        moveInputAction.action.started += MovementAction;
        moveInputAction.action.performed += MovementAction;
        moveInputAction.action.canceled += MovementAction;

        rollInputAction.action.started += RollAction;
        pushInputAction.action.started += PushAction;
        interactInputAction.action.started += InteractAction;
        useInputAction.action.started += UseAction;
    }
    private void OnDisable()
    {
        moveInputAction.action.started -= MovementAction;
        moveInputAction.action.performed -= MovementAction;
        moveInputAction.action.canceled -= MovementAction;

        rollInputAction.action.started -= RollAction;
        pushInputAction.action.started -= PushAction;
        interactInputAction.action.started -= InteractAction;
        useInputAction.action.started -= UseAction;
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

    private void InteractAction(InputAction.CallbackContext obj)
    {
        stateMachine.currentState.InteractAction();
    }

    private void UseAction(InputAction.CallbackContext obj)
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

        Vector3 sphereCenter = transform.position + transform.forward * 1.29f;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(sphereCenter, 1.5f);

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, 1.5f, interactableLayer);

        if (hitColliders.Length > 0)
        {
            foreach (var objCollide in hitColliders)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireCube(objCollide.bounds.center, objCollide.bounds.size);
            }
        }
    }
}
