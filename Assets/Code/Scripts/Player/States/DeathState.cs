using UnityEditor;
using UnityEngine;

public class DeathState : PlayerState
{
    public Transform transform => controller.transform;
    private Rigidbody rb => controller.GetRB();
    public PlayerStateMachine deathStateMachine => stateMachine;

    public bool isSwimming;
    public bool isDead {  get; private set; } = false;

    private float timeDead;

    public override void EnterState()
    {
        isSwimming = true;
        isDead = false;
        timeDead = 0;

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        transform.position = new Vector3(transform.position.x, FishingManager.instance.defaultYPos, transform.position.z);

        FishingManager.instance.AddDeadPlayer(this);

        controller.objectHolder.RemoveItemFromHand();

        controller.animator.SetTrigger("Dead");
        controller.animator.SetBool("Swimming", true);

        controller.transform.SetParent(null);

        AudioManager.instance.Play2dOneShotSound(controller.dieClip, "Objects");

        controller.objectHolder.enabled = false;

    }
    public override void UpdateState()
    {
        controller.animator.SetBool("Moving", controller.movementInput != Vector2.zero);
    }
    public override void FixedUpdateState()
    {
        if (!isSwimming)
            return;
        
        if(isDead)
        {
            timeDead += Time.fixedDeltaTime;

            if (timeDead >= controller.timeToRespawn)
            {
                Respawn();
            }
            return;
        }
        controller.Rotate(controller.movementDirection, controller.swimRotateSpeed);

        Vector3 moveDir = controller.movementInput != Vector2.zero ? transform.forward : Vector3.zero;

        rb.linearVelocity = moveDir * controller.swimSpeed;

    }
    public override void ExitState()
    {
        isDead = false;
        
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        controller.objectHolder.enabled = transform;
        controller.animator.SetBool("Swimming", false);

        FishingManager.instance.RemoveDeadPlayer(this);

        if(ShipsManager.instance.playerShip)
            controller.transform.SetParent(ShipsManager.instance.playerShip.transform);
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
    }
    private void Respawn()
    {
        transform.position = ShipsManager.instance.playerShip.GetSpawnPoints()[1].transform.position;
        stateMachine.ChangeState(stateMachine.idleState);
    }

    
}
