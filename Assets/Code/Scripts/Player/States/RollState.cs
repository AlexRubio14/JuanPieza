using UnityEngine;

public class RollState : PlayerState
{
    private float rollTimePassed;
    private bool bounced;
    private Vector3 rollDir;
    private ParticleSystem particles;
    public override void EnterState()
    {
        rollTimePassed = 0;
        bounced = false;
        controller.animator.SetTrigger("Roll");

        if (controller.movementInput != Vector2.zero && stateMachine.lastState is not DrunkState)
            rollDir = controller.movementDirection;
        else
            rollDir = controller.transform.forward;

        AudioManager.instance.Play2dOneShotSound(controller.dashClip, "Player", 1, 0.95f, 1.05f);

        Vector3 particlesSpawnPos = controller.transform.position;
        particlesSpawnPos.y -= 0.3f;
        particles = GameObject.Instantiate(controller.rollParticles, controller.transform).GetComponent<ParticleSystem>();
        particles.transform.position = particlesSpawnPos;
    }
    public override void UpdateState()
    {

        //Contar el tiempo rodando
        rollTimePassed += Time.deltaTime;
        if(rollTimePassed >= controller.rollDuration)
            stateMachine.ChangeState(stateMachine.lastState);
    }
    public override void FixedUpdateState()
    {
        if(!bounced)
            controller.Movement(rollDir, controller.rollSpeed);
    }
    public override void ExitState()
    {
        if(particles)
            particles.Stop(true);
    }

    public override void RollAction()
    {
        //No puedes volver a rodar
    }
    public override void GrabAction() { }
    public override void ReleaseAction() { /* No deberias tener nada en la mano */}
    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puedes usar ningun objeto*/ }
    public override void StopUseAction() { /*No hace nada*/ }

   
    public override void OnHit(Vector3 _hitPosition, float forceMultiplier = 1) { }
    public override void OnCollisionStay(Collision collision)
    {
        if (bounced || !collision.gameObject.CompareTag("Scenario"))
            return;

        Bounce(collision);
    }


    private void Bounce(Collision _collision)
    {
        Vector3 bounceNormal = new Vector3(_collision.contacts[0].normal.x, 0, _collision.contacts[0].normal.z);

        //Rebotar
        if (bounceNormal.x == 0 && bounceNormal.z == 0)
            return;

        Vector3 bounceDir = new Vector3(_collision.contacts[0].normal.x, 0, _collision.contacts[0].normal.z) * controller.bounceForce.x + Vector3.up * controller.bounceForce.y;
        bounced = true;
        controller.rb.linearVelocity = Vector3.zero;
        controller.AddImpulse(bounceDir, controller.rollSpeed);
        rollTimePassed = -controller.rollDuration / 2;
        controller.animator.SetTrigger("Hitted");
        AudioManager.instance.Play2dOneShotSound(controller.dashHitClip, "Player", 0.7f, 0.95f, 1.05f);

        GameObject bounceParticles = GameObject.Instantiate(controller.rollBounceParticles, _collision.contacts[0].point, Quaternion.identity);
        bounceParticles.transform.forward = bounceNormal;

        particles.Stop(true);
    }
}
