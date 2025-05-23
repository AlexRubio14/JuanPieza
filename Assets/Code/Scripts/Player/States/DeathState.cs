using UnityEngine;

public class DeathState : PlayerState
{
    public Transform transform => controller.transform;
    private Rigidbody rb => controller.GetRB();
    public PlayerStateMachine deathStateMachine => stateMachine;

    public bool isSwimming;
    public bool isDead {  get; private set; } = false;

    public float timeAtWater {  get; private set; }
    private float timeDead;

    private float respawnDuration = 2;
    private float respawnProcess = 0;
    private float respawnHeight = 6;
    public bool isRespawning = false;
    private Vector3 startRespawnPos;
    private Vector3 endRespawnPos;
    private ParticleSystem respawnParticles;

    private AudioSource swimSource;

    public override void EnterState()
    {
        controller.objectHolder.RemoveItemFromHand();


        isSwimming = true;
        isDead = false;
        isRespawning = false;

        respawnProcess = 0;
        timeDead = 0;
        timeAtWater = 0;

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        transform.position = new Vector3(transform.position.x, FishingManager.instance.defaultYPos, transform.position.z);

        FishingManager.instance.AddDeadPlayer(this);

        controller.animator.SetTrigger("Dead");
        controller.animator.SetBool("Swimming", true);

        controller.transform.SetParent(null);

        AudioManager.instance.Play2dOneShotSound(controller.dieClip, "Objects");

        controller.objectHolder.enabled = false;

    }
    public override void UpdateState()
    {
        if (isRespawning)
        {
            respawnProcess += Time.deltaTime / respawnDuration;
            controller.transform.position = Vector3.Lerp(startRespawnPos, endRespawnPos, respawnProcess);
            if(respawnProcess >= 0.7f)
                controller.animator.SetBool("Swimming", false);
            if (respawnProcess >= 1)
                stateMachine.ChangeState(stateMachine.idleState);
        }
        else if (isSwimming && !isDead)
        {
            controller.animator.SetBool("Moving", controller.movementInput != Vector2.zero);
            timeAtWater += Time.deltaTime;

            if(controller.movementInput != Vector2.zero && (!swimSource || !swimSource.isPlaying))
            {
                swimSource = AudioManager.instance.Play2dLoop(controller.swimClip, "Player", 0.6f, 0.95f, 1.05f);
            }
        }

        //Esta comprobacion es para que evitar el bug del sonido de nadar infinito
        if ((isDead || isRespawning ) && swimSource && swimSource.isPlaying)
        {
            AudioManager.instance.StopLoopSound(swimSource);
        }


    }
    public override void FixedUpdateState()
    {
        if (!isSwimming)
            return;
        
        if(isDead && !isRespawning)
        {
            timeDead += Time.fixedDeltaTime;

            if (timeDead >= controller.timeToRespawn)
                StartRespawn();

            return;
        }
        float rotationSpeed = controller.swimRotateSpeed;
        if (isRespawning)
            rotationSpeed = controller.rotationSpeed;
        controller.Rotate(controller.movementDirection, rotationSpeed);

        Vector3 moveDir = controller.movementInput != Vector2.zero ? transform.forward : Vector3.zero;

        rb.linearVelocity = moveDir * controller.swimSpeed;

    }
    public override void ExitState()
    {
        controller.animator.SetBool("Swimming", false);
        AudioManager.instance.StopLoopSound(swimSource);

        isDead = false;
        isSwimming = false;
        isRespawning = false;

        if (rb)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.linearVelocity = Vector3.zero;
        }
        
        if(controller)
            controller.objectHolder.enabled = true;
        
        if(FishingManager.instance)
            FishingManager.instance.RemoveDeadPlayer(this);

        if(respawnParticles)
            respawnParticles.Stop(true);
    }

    public override void RollAction() { /*No puedes rodar*/ }
    public override void GrabAction() { /* Esta agarrando algo no puedes coger nada mas */ }
    public override void ReleaseAction() { }
    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puedes usar ningun objeto*/ }
    public override void StopUseAction() { /*No hace nada*/ }

    public override void OnHit(Vector3 _hitPosition, float forceMultiplier = 1) { /*No puedes ser golpeado */ }
    public override void OnCollisionEnter(Collision collision) { }

   
    public void KillPlayer()
    {
        transform.position = new Vector3(-100, -100, -100);
        isDead = true;

        if (swimSource)
        {
            AudioManager.instance.StopLoopSound(swimSource);
        }
    }
    public void StartRespawn()
    {
        isRespawning = true;

        startRespawnPos = ShipsManager.instance.playerShip.GetSpawnPoints()[controller.playerInput.playerReference].transform.position;
        startRespawnPos.y = respawnHeight;
        Physics.Raycast(startRespawnPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, controller.slopeCheckLayer);

        endRespawnPos = hit.point + Vector3.up * controller.GetComponent<CapsuleCollider>().height / 2;

        respawnParticles = GameObject.Instantiate(controller.respawnParticles, hit.point, Quaternion.identity).GetComponent<ParticleSystem>();

        respawnParticles.transform.position = hit.point;
        respawnParticles.Play(true);

        controller.animator.SetBool("Moving", false);

        AudioManager.instance.Play2dOneShotSound(controller.respawnClip, "Objects", 0.3f, 0.95f, 1.05f);
    }

}
