using UnityEngine;

public class CannonState : PlayerState
{
    private Weapon currentWeapon;

    public override void EnterState()
    {
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
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer daño
    }  

    private void MoveCannon()
    {
        if (controller.movementInput.x != 0)
            controller.Movement(controller.transform.forward, controller.cannonSpeed * controller.movementInput.x);
        if (controller.movementInput.y != 0)
            controller.Rotate(controller.transform.right * controller.movementInput.y, controller.cannonRotationSpeed);
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
