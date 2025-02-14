using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CannonState : PlayerState
{
    private Weapon currentWeapon;

    public override void EnterState()
    {
        controller.animator.SetBool("OnCannon", true);
    }
    public override void UpdateState()
    {
        
    }
    public override void FixedUpdateState()
    {
        MoveCannon();
        TiltCannon();
    }
    public override void ExitState()
    {
        controller.animator.SetBool("OnCannon", false);
    }
    public override void RollAction() { /*No puedes rodar*/ }
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
        controller.Use();
        controller.animator.SetTrigger("Shoot");
    }
    public override void StopUseAction() 
    { 
        controller.StopUse();
    }

    public override void OnHit(Vector3 _hitPosition)
    {
        //controller.Interact(); // Bajarse del cañon
        base.OnHit(_hitPosition);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer da�o
    }  

    private void MoveCannon()
    {

        if (controller.objectHolder.hintController.deviceType == HintController.DeviceType.KEYBOARD)
            KeyboardCannonMovement();
        else
            GamepadCannonMovement();
    }

    private void KeyboardCannonMovement()
    {
        Vector3 moveDir;

        controller.animator.SetBool("Moving", controller.movementInput.y != 0);

        if (controller.movementInput.y != 0)
        {
            if (controller.CheckSlope())
                moveDir = controller.GetSlopeMoveDir(controller.transform.forward);
            else
                moveDir = controller.transform.forward;

            controller.Movement(moveDir, controller.cannonSpeed * controller.movementInput.y);
        }

        if (controller.movementInput.x != 0)
            controller.Rotate(controller.transform.right * controller.movementInput.x, controller.cannonRotationSpeed);
    }
    private void GamepadCannonMovement()
    {

        if (Mathf.Abs(controller.movementInput.y) > Mathf.Abs(controller.movementInput.x))
        {
            Vector3 moveDir;
            if (controller.CheckSlope())
                moveDir = controller.GetSlopeMoveDir(controller.transform.forward);
            else
                moveDir = controller.transform.forward;

            controller.Movement(moveDir, controller.cannonSpeed * controller.movementInput.y);
            controller.animator.SetBool("Moving", true);
        }
        else if (Mathf.Abs(controller.movementInput.y) < Mathf.Abs(controller.movementInput.x))
        {
            controller.Rotate(controller.transform.right * controller.movementInput.x, controller.cannonRotationSpeed);
            controller.animator.SetBool("Moving", false);
        }
    }

    private void TiltCannon()
    {
        if (controller.cannonTilt == 0)
            return;

        currentWeapon.tiltProcess = Mathf.Clamp01(currentWeapon.tiltProcess + controller.cannonTilt * currentWeapon.tiltSpeed * Time.fixedDeltaTime);
        currentWeapon.tiltObject.localRotation = Quaternion.Lerp(
            Quaternion.Euler(currentWeapon.minWeaponTilt),
            Quaternion.Euler(currentWeapon.maxWeaponTilt), 
            currentWeapon.tiltProcess * currentWeapon.tiltSpeed
            );
    }
    public void SetWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }
}
