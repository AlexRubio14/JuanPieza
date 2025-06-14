using System.Collections.Generic;
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
    public float maxSlopeAngle;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    public LayerMask slopeCheckLayer;
    private RaycastHit slopeHit;


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
    public GameObject rollParticles {  get; private set; }
    [field: SerializeField]
    public GameObject rollBounceParticles {  get; private set; }
    [field: SerializeField]
    public RumbleController.RumblePressets bounceRumble {  get; private set; }

    [field: Space, Header("Push"), SerializeField]
    public float pushRadius { get; private set; }
    [field: SerializeField]
    public float pushOffset { get; private set; }
    [field: SerializeField]
    public float pushCD {  get; private set; }
    [HideInInspector]
    public bool canPush;
    [field: SerializeField]
    public LayerMask pushLayers { get; private set; }
    [field: SerializeField]
    public Vector2 playerPushForce { get; private set; }
    [field: SerializeField]
    public Vector2 objectPushForce { get; private set; }
    [field: SerializeField]
    public float objectTorqueForce {  get; private set; }
    [field: SerializeField] public float pirateKnockbackForce { get; private set; }
    [field: SerializeField] public float pirateUpForce { get; private set; }
    [SerializeField] public LayerMask objectLayer {  get; set; }


    [field: Space, Header("Death"), SerializeField]
    public float timeToDie { get; private set; }
    [field: SerializeField]
    public float timeToRespawn { get; private set; }
    [field: SerializeField]
    public float swimSpeed { get; private set; }
    [field: SerializeField]
    public float swimRotateSpeed { get; private set; }
    [field: SerializeField]
    public GameObject respawnParticles { get; private set; }
    [field: SerializeField]
    public AudioClip swimClip {  get; private set; }
    [field: SerializeField]
    public AudioClip respawnClip {  get; private set; }


    [field: Space, Header("Weapon"), SerializeField]
    public float weaponSpeed { get; private set; }
    [field:SerializeField]
    public float weaponRotationSpeed { get; private set; }
    [field:SerializeField]
    public float weaponRotationOffset { get; private set; }
    public Vector3 weaponRotationDir { get; private set; }


    [field: Space, Header("Drunk"), SerializeField]
    public float baseDrunkAngle {  get; private set; }
    [field: SerializeField]
    public float drunkAngleIncrement { get; private set; }
    [field: SerializeField]
    public float baseDrunkLookAtSpeed {  get; private set; }
    [field: SerializeField]
    public float drunkLookAtIncrement { get; private set; }
    [field: SerializeField]
    public float drunkMinAngleDiff { get; private set; }
    [field: SerializeField]
    public float drunkStateDuration {  get; private set; }
    [field: SerializeField]
    public ParticleSystem drunkParticles { get; private set; }


    [field: Space, Header("Dance"), SerializeField]
    public AudioClip danceMusic { get; private set; }


    [Space, Header("Ice"), SerializeField] 
    private float iceDrag;
    private float realDrag;
    private bool isOnIce;


    [field: Space, Header("Audio"), SerializeField]
    public AudioClip dieClip;
    [SerializeField] public AudioClip dashClip;
    [SerializeField] public AudioClip dashHitClip;
    [SerializeField] public AudioClip pushGameObjectClip;
    [SerializeField] public List<AudioClip> pushListClips;


    public Animator animator { get; private set; }
    private CapsuleCollider capsuleCollider;
    public Rigidbody rb { get; private set; }

    public ObjectHolder objectHolder { get; private set; }

    public bool movementBuffActive {  get; set; }
    public float currentKnockBackTime { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
        objectHolder = GetComponentInChildren<ObjectHolder>();

        stateMachine = GetComponent<PlayerStateMachine>();
        stateMachine.InitializeStates(this);
        realDrag = rb.linearDamping;
    }

    private void Start()
    {
        SuscribeActions();
        canRoll = true;
        canPush = true;
        objectHolder = GetComponentInChildren<ObjectHolder>();
        drunkParticles.Stop(true);
    }
    private void OnEnable()
    {
        SuscribeActions();
        PlayersManager.instance.ingamePlayers.Add(this);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(stateMachine.deathState);
            stateMachine.deathState.StartRespawn();
        }

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

        playerInput.OnStopUseAction += StopUseAction;

        playerInput.OnDanceAction += DanceAcion;

        playerInput.OnGrabAction += GrabAction;
        
        playerInput.OnReleaseAction += ReleaseAction;

        playerInput.OnPushAction += PushAction;

        playerInput.OnWeaponRotateAction += WeaponRotateAction;
    }

    private void OnDisable()
    {
        playerInput.OnMoveAction -= MovementAction;

        playerInput.OnRollAction -= RollAction;

        playerInput.OnInteractAction -= InteractAction;

        playerInput.OnStopInteractAction -= StopInteractAction;

        playerInput.OnUseAction -= UseAction;

        playerInput.OnDanceAction -= DanceAcion;

        playerInput.OnGrabAction -= GrabAction;

        playerInput.OnReleaseAction -= ReleaseAction;

        playerInput.OnPushAction -= PushAction;

        playerInput.OnWeaponRotateAction -= WeaponRotateAction;


        movementInput = Vector2.zero;
        movementDirection = Vector3.zero;
        stateMachine.ChangeState(stateMachine.idleState);
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
        if (canRoll && !objectHolder.GetHasObjectPicked() && OnFloor())
        {
            stateMachine.currentState.RollAction();
            canRoll = false;
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
    private void StopUseAction()
    {
        stateMachine.currentState.StopUseAction();
    }
    private void PushAction()
    {
        stateMachine.currentState.PushAction();
    }
    public void PlayerHitted(Vector3 _hitPosition, float forceMultiplier = 1)
    {
        stateMachine.currentState.OnHit(_hitPosition, forceMultiplier);
    }
    private void DanceAcion()
    {
        if (stateMachine.currentState is IdleState)
            stateMachine.ChangeState(stateMachine.danceState);
    }
    private void GrabAction()
    {
        stateMachine.currentState.GrabAction();
    } 
    private void ReleaseAction()
    {
        stateMachine.currentState.ReleaseAction();
    }
    private void WeaponRotateAction(Vector2 _direction)
    {
        weaponRotationDir = new Vector3(_direction.x, 0, _direction.y);
    }

    #endregion

    #region Actions
    public void Movement(Vector3 _direction, float _speed)
    {
        rb.AddForce(_direction * _speed, ForceMode.Force);
    }

    public bool CheckSlope()
    {
        float angle = 0;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, capsuleCollider.height / 2 + slopeCheckDistance, slopeCheckLayer))
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);

        if( angle == 0 && 
            Physics.Raycast(transform.position + transform.forward * capsuleCollider.radius, Vector3.down, out slopeHit, capsuleCollider.height / 2 + slopeCheckDistance, slopeCheckLayer))
            angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;
    }
    public Vector3 GetSlopeMoveDir(Vector3 _movementDir)
    {
        Vector3 slopeDir = Vector3.ProjectOnPlane(_movementDir, slopeHit.normal).normalized;
        if (slopeDir.y < -0.1)
            slopeDir /= 2;
        Debug.DrawLine(transform.position, transform.position +  slopeDir * 2);
        return slopeDir;
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
    private void WaitCanPush()
    {
        canPush = true;
    }
    public void Grab()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();
        InteractableObject nearObject = objectHolder.GetNearestInteractableObject();
        if (!handObject && nearObject && nearObject.CanGrab(objectHolder))
            nearObject.Grab(objectHolder);
    }
    public void Release()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();
        if (handObject)
            handObject.Release(objectHolder);
    }
    public void Interact()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();
        InteractableObject nearestObject = objectHolder.GetNearestInteractableObject();
        
        if (!handObject && (!nearestObject || nearestObject && !nearestObject.CanInteract(objectHolder)))
            return;

        if(handObject && handObject.CanInteract(objectHolder))
        {
            handObject.Interact(objectHolder);
        }else if (nearestObject && nearestObject.CanInteract(objectHolder))
        {
            nearestObject.Interact(objectHolder);
            objectHolder.ChangeNearestInteractableObject(null);
        }
        
        animator.SetBool("Pick", objectHolder.GetHandInteractableObject());

    }
    public void StopInteract()
    {
        InteractableObject currentObject = objectHolder.GetHandInteractableObject();
        
        if(currentObject == null)
            currentObject = objectHolder.GetNearestInteractableObject();

        if(currentObject && currentObject.CanInteract(objectHolder))
        {
            currentObject.StopInteract(objectHolder);
        }
    }
    public void Use()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();

        if (!handObject)
            return;

        handObject.Use(objectHolder);
        InteractableObject newHandObject = objectHolder.GetHandInteractableObject();
        InteractableObject nearestObj = objectHolder.GetNearestInteractableObject();
        if (nearestObj && nearestObj.CanInteract(objectHolder))
        {
            nearestObj.GetSelectedVisual().Show();
        }
    }
    public void StopUse()
    {
        InteractableObject currentObject = objectHolder.GetHandInteractableObject();

        if (currentObject)
        {
            currentObject.StopUse(objectHolder);
        }
    }
    #endregion


    public bool OnFloor()
    {
        Vector3 raycastPos = transform.position;
        raycastPos.y -= capsuleCollider.height / 2 - 0.1f;

        //Tirar Raycast contra el suelo
        if (Physics.Raycast(raycastPos, Vector3.down, 0.3f, slopeCheckLayer))
            return true;

        raycastPos.x += capsuleCollider.radius;
        raycastPos.z += capsuleCollider.radius;

        float multiplyX = 1;
        float multiplyZ = 1;

        for (int i = 1; i<= 4; i++)
        {
            if (Physics.Raycast(raycastPos, Vector3.down, 0.3f, slopeCheckLayer))
                return true;

            raycastPos.x = transform.position.x + capsuleCollider.radius * multiplyX;
            multiplyX *= -1;

            if(i % 2 == 0)
            {
                multiplyZ *= -1;
                raycastPos.z = transform.position.z + capsuleCollider.radius * multiplyZ;
            }
        }

        return false;
    }
    public Rigidbody GetRB() { return rb; }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Colisiona contra " + collision.contacts[0].otherCollider.gameObject.name + " | El estado es " + stateMachine.currentState.ToString());
        stateMachine.currentState.OnCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        stateMachine.currentState.OnCollisionStay(collision);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmo del area de empujar
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.forward * pushOffset, pushRadius);

        //Gizmos del forward del personaje
        Gizmos.color = Color.blue;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + transform.forward * 2;
        Gizmos.DrawLine(startPos, endPos);

        if (capsuleCollider)
        {
            Gizmos.color = Color.green;
            startPos = transform.position;
            startPos.y -= capsuleCollider.height / 2 - 0.1f;
            endPos = startPos + Vector3.down * 0.3f;
            Gizmos.DrawLine(startPos, endPos);

            startPos.x += capsuleCollider.radius;
            startPos.z += capsuleCollider.radius;
            endPos.x += capsuleCollider.radius;
            endPos.z += capsuleCollider.radius;

            float multiplyX = 1;
            float multiplyZ = 1;

            for (int i = 1; i < 5; i++)
            {

                startPos.x = transform.position.x + capsuleCollider.radius * multiplyX;
                endPos.x = startPos.x;

                multiplyX *= -1;
                if (i % 2 == 0)
                {
                    multiplyZ *= -1;
                    startPos.z = transform.position.z + capsuleCollider.radius * multiplyZ;
                    endPos.z = startPos.z;

                }
                Gizmos.DrawLine(startPos, endPos);

            }
        }

        Gizmos.color = Color.red;


        Quaternion drunkRotation = Quaternion.Euler(0, baseDrunkAngle, 0);
        Vector3 drunkDirection = drunkRotation * transform.forward;
        Vector3 drunkEndPos1 = transform.position + drunkDirection * 3;
        Gizmos.DrawLine(transform.position, drunkEndPos1);

        drunkRotation = Quaternion.Euler(0, -baseDrunkAngle, 0);
        drunkDirection = drunkRotation * transform.forward;
        Vector3 drunkEndPos2 = transform.position + drunkDirection * 3;
        Gizmos.DrawLine(transform.position, drunkEndPos2);

        Gizmos.DrawLine(drunkEndPos1, drunkEndPos2);

        Gizmos.color = Color.green;

        drunkRotation = Quaternion.Euler(0, drunkMinAngleDiff / 2, 0);
        drunkDirection = drunkRotation * transform.forward;
        Vector3 drunkEndPos = transform.position + drunkDirection * 3;
        Gizmos.DrawLine(transform.position, drunkEndPos);

        drunkRotation = Quaternion.Euler(0, -drunkMinAngleDiff / 2, 0);
        drunkDirection = drunkRotation * transform.forward;
        drunkEndPos = transform.position + drunkDirection * 3;
        Gizmos.DrawLine(transform.position, drunkEndPos);
    }

    public void SetOnIce(bool value)
    {
        if (value == isOnIce)
            return;

        isOnIce = value;
        if (isOnIce)
            rb.linearDamping = iceDrag;
        else
            rb.linearDamping = realDrag;

    }

    public void SetBaseMovementSpeed(float speed)
    {
        baseMovementSpeed = speed;
    }

}
