using UnityEngine;

public class PushState : PlayerState
{
    private float pushTimePassed;
    public override void EnterState()
    {
        if(!controller.canPush)
            return;

        controller.canPush = false;

        controller.animator.SetTrigger("Push");
        pushTimePassed = 0;

        Vector3 positionToCast = controller.transform.position + controller.transform.forward * controller.pushOffset;
        RaycastHit[] hits = Physics.SphereCastAll(positionToCast, controller.pushRadius, controller.transform.forward,
            1f, controller.pushLayers);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider && controller.gameObject != hit.collider.gameObject)
            {
                Vector2 pushForce = Vector2.zero;
                if (hit.collider.CompareTag("Object"))
                {
                    pushForce = controller.objectPushForce;
                    hit.rigidbody.AddTorque(controller.transform.right * controller.objectTorqueForce, ForceMode.Impulse);
                }
                else if (hit.collider && hit.collider.CompareTag("Player"))
                {


                    pushForce = controller.playerPushForce;
                    PlayerController hittedPlayer = hit.collider.GetComponent<PlayerController>();
                    if (hittedPlayer.stateMachine.currentState is DeathState or StunedState)
                        continue;

                    hittedPlayer.animator.SetTrigger("Hitted");
                    if (hittedPlayer.stateMachine.currentState is FishingState)
                    {
                        hittedPlayer.objectHolder.GetHandInteractableObject().Release(hittedPlayer.objectHolder);
                        hittedPlayer.stateMachine.ChangeState(hittedPlayer.stateMachine.idleState);
                    }

                }

                Vector3 pushForward = controller.transform.forward * pushForce.x;
                Vector3 pushUp = Vector3.up * pushForce.y;
                hit.rigidbody.AddForce(pushForward + pushUp, ForceMode.Impulse);

                AudioManager.instance.Play2dOneShotSound(controller.pushGameObjectClip, "Player", 1, 0.85f, 1.15f);
            }


            if (hit.collider && hit.collider.CompareTag("BoardingPirate"))
            {
                hit.transform.gameObject.TryGetComponent(out PirateBoardingController pirateController);
                pirateController.ChangeState(PirateBoardingController.PirateState.KNOCKBACK);
                Vector3 knockbackForward = controller.transform.forward * controller.pirateKnockbackForce;
                Vector3 pushUp = Vector3.up * controller.pirateUpForce;

                Vector3 knockbackDir = knockbackForward + pushUp;

                pirateController.PirateKnockback(knockbackDir, controller.pirateKnockbackForce);
                AudioManager.instance.Play2dOneShotSound(controller.pushGameObjectClip, "Player", 1, 0.85f, 1.15f);
            }
        }

        controller.Invoke("WaitCanPush", controller.pushCD);


        PickRandomPushClip();
    }

    public void PickRandomPushClip()
    {
        int randIndex = Random.Range(0, controller.pushListClips.Count);

        AudioManager.instance.Play2dOneShotSound(controller.pushListClips[randIndex], "Player", 1f, 0.9f, 1.1f);
    }

    public override void UpdateState()
    {
        //Contar el tiempo rodando
        pushTimePassed += Time.deltaTime;
        if (pushTimePassed >= controller.rollDuration)
        {
            stateMachine.ChangeState(stateMachine.lastState);
        }
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {

    }

    public override void RollAction()
    {
        //No puedes rodar
    }
    public override void GrabAction() { /* No puedes coger nada */ }
    public override void ReleaseAction() { /* No deberias tener nada en la mano */}
    public override void InteractAction() { /*No puede interactuar aqui*/ }
    public override void StopInteractAction() { /*No hace nada*/ }
    public override void UseAction() { /*No puede usar nada aqui*/ }
    public override void StopUseAction() { /*No hace nada*/ }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }


}
