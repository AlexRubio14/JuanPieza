using UnityEngine;

public class DrunkState : PlayerState
{
    float desiredDrunkAngle;
    float drunkAngle;
    float currentDrunkAngle;
    float currentLookAtSpeed;

    float currentTimeDrunk = 0;
    public override void EnterState()
    {
        controller.animator.SetBool("Moving", true);
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;
        controller.rb.useGravity = true;
        controller.drunkParticles.Play(true);

        if (currentTimeDrunk == 0)
        {
            currentDrunkAngle = controller.baseDrunkAngle;
            currentLookAtSpeed = controller.baseDrunkLookAtSpeed;
            drunkAngle = 0;
            GenerateDesiredAngle();
            ParticleSystem.EmissionModule emission = controller.drunkParticles.emission;
            emission.rateOverTime = 1;
        }

    }
    public override void UpdateState()
    {
        if(controller.movementDirection != Vector3.zero)
            drunkAngle = Mathf.Lerp(drunkAngle, desiredDrunkAngle, Time.deltaTime * currentLookAtSpeed);
        
        if (Mathf.Abs(desiredDrunkAngle - drunkAngle) <= controller.drunkMinAngleDiff)
            GenerateDesiredAngle();

        currentTimeDrunk += Time.deltaTime;
        if (currentTimeDrunk >= controller.drunkStateDuration)
            EndBeerEffect();
    }
    public override void FixedUpdateState()
    {
        if (controller.movementDirection == Vector3.zero)
        {   
            controller.animator.SetBool("Moving", false);
            return;
        }

        controller.animator.SetBool("Moving", true);

        Vector3 moveDir = CalculateCurrentDirection();
        controller.Rotate(moveDir, controller.rotationSpeed);
        controller.Movement(moveDir, controller.baseMovementSpeed);
    }
    public override void ExitState()
    {
        controller.rb.constraints = RigidbodyConstraints.FreezeRotation;
        controller.drunkParticles.Stop(false);
    }

    public override void RollAction()
    {
        stateMachine.ChangeState(stateMachine.rollState);
    }
    public override void GrabAction()
    {
        controller.Grab();
    }
    public override void ReleaseAction()
    {
        controller.Release();
    }
    public override void InteractAction()
    {
        controller.Interact();
    }
    public override void StopInteractAction()
    {
        controller.StopInteract();
    }
    public override void UseAction()
    {
        if (controller.objectHolder.GetHandInteractableObject())
            controller.Use();
        else
            stateMachine.ChangeState(stateMachine.pushState);
    }
    public override void StopUseAction()
    {
        controller.StopUse();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    private void GenerateDesiredAngle()
    {
        desiredDrunkAngle = Random.Range(-currentDrunkAngle, currentDrunkAngle);
    }
    private Vector3 CalculateCurrentDirection()
    {
        Quaternion drunkRotation = Quaternion.Euler(0, drunkAngle, 0);
        Vector3 moveDir = controller.movementDirection != Vector3.zero ? controller.movementDirection : controller.transform.forward;
        return (drunkRotation * moveDir).normalized;
    }
    private void EndBeerEffect()
    {
        if (currentDrunkAngle == controller.baseDrunkAngle)
        {
            currentTimeDrunk = 0;
            stateMachine.ChangeState(stateMachine.idleState);
        }
        else
        {
            currentTimeDrunk = 0;
            currentDrunkAngle -= controller.drunkAngleIncrement;
            currentLookAtSpeed -= controller.drunkLookAtIncrement;
            ParticleSystem.EmissionModule emission = controller.drunkParticles.emission;
            emission.rateOverTime = controller.drunkParticles.emission.rateOverTime.constant - 1;
        }
    }
    public void DrinkBeer()
    {
        currentDrunkAngle += controller.drunkAngleIncrement;
        currentLookAtSpeed += controller.drunkLookAtIncrement;
        ParticleSystem.EmissionModule emission = controller.drunkParticles.emission;
        emission.rateOverTime = controller.drunkParticles.emission.rateOverTime.constant + 1;
        currentTimeDrunk = 0;
    }
    
}
