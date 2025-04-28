using UnityEngine;

public class CannonState : PlayerState
{
    private Weapon currentWeapon;
    private Vector3 weaponOffset;

    private float currentAngle;
    public override void EnterState()
    {
        currentWeapon.rb.constraints = RigidbodyConstraints.FreezeRotation;

        CalculateCannonDistance();
        
        controller.animator.SetBool("OnCannon", true);
        controller.animator.SetBool("Pick", true);
        currentAngle = currentWeapon.transform.rotation.eulerAngles.y;
    }
    public override void UpdateState()
    {
        currentWeapon.transform.position = controller.transform.position + controller.transform.forward * controller.weaponRotationOffset + new Vector3(0, weaponOffset.y, 0);
    }
    public override void FixedUpdateState()
    {
        if (currentWeapon.isTilting)
            return;

        MoveCannon();
        RotateCannon();
    }
    public override void ExitState()
    {
        if(currentWeapon)
            currentWeapon.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        controller.animator.SetBool("OnCannon", false);
        controller.animator.SetBool("Pick", false);
    }
    public override void RollAction() { /*No puedes rodar*/ }
    public override void GrabAction() { /* Esta agarrando algo no puedes coger nada mas */ }

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
    public override void UseAction() { }
    public override void StopUseAction() 
    { 
        controller.StopUse();
    }

    public override void OnHit(Vector3 _hitPosition, float forceMultiplier = 1)
    {
        //controller.Interact(); // Bajarse del cañon
        base.OnHit(_hitPosition);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer da�o
    }  

    private void CalculateCannonDistance()
    {
        Vector3 weaponDistance = currentWeapon.transform.position - controller.transform.position;

        Vector3 newPlayerForward = 
            (
                new Vector3(currentWeapon.transform.position.x, 0, currentWeapon.transform.position.z) 
                - new Vector3(controller.transform.position.x, 0, controller.transform.position.z)
            ).normalized;

        controller.transform.forward = newPlayerForward;


        Vector3 newPos;
        if (weaponDistance.magnitude <= 2f)
            newPos = weaponDistance + controller.transform.forward * currentWeapon.rideOffset;
        else
        {
            newPos = weaponDistance;
            Vector3 playerPos = new Vector3(controller.transform.position.x, currentWeapon.transform.position.y, controller.transform.position.z);

            currentWeapon.transform.position = playerPos + newPlayerForward * 2;
            //controller.transform.position = weaponPos - newPlayerForward * 2;
        }

        weaponOffset = newPos;
    }
    private void MoveCannon()
    {
        controller.animator.SetBool("Moving", controller.movementInput != Vector2.zero);


        if (controller.movementDirection != Vector3.zero)
            controller.Movement(controller.movementDirection, controller.weaponSpeed);
    }
    private void RotateCannon()
    {
        if (controller.weaponRotationAxis == 0)
            return;

        currentAngle += controller.weaponRotationAxis * (controller.weaponRotationSpeed * Time.fixedDeltaTime);
        currentAngle %= 360;
        currentWeapon.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
        
    }
    public void SetWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }
}
