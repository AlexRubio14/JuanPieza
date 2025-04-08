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
    }
    public override void UpdateState()
    {

    }
    public override void FixedUpdateState()
    {
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

        FishingManager.instance.RemoveDeadPlayer(this);
        
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        controller.animator.SetBool("Swimming", false);
        
        if(ShipsManager.instance.playerShip)
            controller.transform.SetParent(ShipsManager.instance.playerShip.transform);
    }

    public override void RollAction() { /*No puedes rodar*/ }

    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puedes usar ningun objeto*/ }
    public override void StopUseAction() { /*No hace nada*/ }

    public override void OnHit(Vector3 _hitPosition) { /*No puedes ser golpeado */ }
    public override void OnCollisionEnter(Collision collision) { }

   
    public void KillPlayer()
    {
        transform.position = new Vector3(-100, -100, -100);
        isDead = true;
    }
    private void Respawn()
    {
        transform.position = ShipsManager.instance.playerShip.transform.position + new Vector3(0f, 2f, 0f);
        stateMachine.ChangeState(stateMachine.idleState);
    }

    
}
