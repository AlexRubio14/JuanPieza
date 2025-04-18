using UnityEngine;

public class DanceState : PlayerState
{
    private AudioSource danceMusicAS;
    public override void EnterState()
    {
        //Empezar musica
        danceMusicAS = AudioManager.instance.Play2dLoop(controller.danceMusic, "DanceMusic", 1, 1, 1);

        controller.animator.SetTrigger("Dance");
        controller.animator.SetBool("Dancing", true);
    }

    public override void UpdateState()
    {
        if (controller.movementInput != Vector2.zero)
            stateMachine.ChangeState(stateMachine.moveState);
    }
    public override void FixedUpdateState() { }
    public override void ExitState()
    {
        //Parar la musica
        AudioManager.instance.StopLoopSound(danceMusicAS);
        controller.animator.SetBool("Dancing", false);
    }

    public override void RollAction() { }
    public override void InteractAction() { }
    public override void StopInteractAction() { }

    public override void UseAction() { }
    public override void StopUseAction() { }


    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
