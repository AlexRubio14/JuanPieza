using UnityEngine;

public class DanceState : PlayerState
{
    private AudioSource danceMusicAS;
    public override void EnterState()
    {
        //Empezar musica
        if (!AudioManager.instance.radioAs.isPlaying && !AudioManager.instance.danceAs.isPlaying)
        {
            AudioManager.instance.danceAs.clip = controller.danceMusic;
            AudioManager.instance.danceAs.Play();
        }
        
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
        if(AudioManager.instance.danceAs.isPlaying)
            AudioManager.instance.danceAs.Stop();

        controller.animator.SetBool("Dancing", false);
    }

    public override void RollAction() { }
    public override void GrabAction() { /* Esta agarrando algo no puedes coger nada mas */ }
    public override void ReleaseAction()
{
        controller.Release();
    }
    public override void InteractAction() { }
    public override void StopInteractAction() { }

    public override void UseAction() { }
    public override void StopUseAction() { }


    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
