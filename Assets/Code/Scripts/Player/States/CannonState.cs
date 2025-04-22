using UnityEngine;

public class CannonState : PlayerState
{
    private Weapon currentWeapon;
    private Vector3 weaponOffset;

    private bool isRotating;
    private float currentAngle;
    public override void EnterState()
    {
        currentWeapon.rb.constraints = RigidbodyConstraints.FreezeRotation;

        CalculateCannonDistance();
        currentWeapon.transform.position = controller.transform.position + weaponOffset;
        controller.animator.SetBool("OnCannon", true);
        controller.animator.SetBool("Pick", true);
        isRotating = false;
        currentAngle = currentWeapon.transform.rotation.eulerAngles.y;
    }
    public override void UpdateState()
    {
        currentWeapon.transform.position = controller.transform.position + controller.transform.forward * controller.cannonRotationOffset + new Vector3(0, weaponOffset.y, 0);
    }
    public override void FixedUpdateState()
    {
        if(!isRotating)
            MoveCannon();
        else
            RotateCannon();
    }
    public override void ExitState()
    {
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
    public override void UseAction()
    {
        controller.Use();
        controller.animator.SetTrigger("Shoot");
    }
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
        Vector3 weaponDirection = weaponDistance.normalized;

        Vector3 newPos;
        float dot = Vector3.Dot(weaponDirection, controller.transform.forward);
        if (dot > 0.5f)
        {
            if (weaponDistance.magnitude <= 2f)
                newPos = weaponDistance + controller.transform.forward * currentWeapon.rideOffset;
            else
                newPos = weaponDistance;
        }
        else
        {
            newPos = controller.transform.forward * weaponDistance.magnitude + controller.transform.forward * currentWeapon.rideOffset;
            newPos.y = weaponDistance.y;
        }



        weaponOffset = newPos;
    }
    private void MoveCannon()
    {
        controller.animator.SetBool("Moving", controller.movementInput != Vector2.zero);


        if (controller.movementDirection != Vector3.zero)
            controller.Movement(controller.movementDirection, controller.cannonSpeed);
    }
    private void RotateCannon()
    {
        if (controller.movementInput.x == 0)
            return;
        currentAngle += controller.movementInput.x * (controller.cannonRotationSpeed * Time.fixedDeltaTime);
        currentAngle %= 360;
        currentWeapon.transform.rotation = Quaternion.Euler(0, currentAngle, 0);
        
    }
    public void SetWeapon(Weapon _weapon)
    {
        currentWeapon = _weapon;
    }
    public void SwapIsRotating()
    {
        isRotating = !isRotating;
        currentAngle = currentWeapon.transform.rotation.eulerAngles.y;
    }
}
