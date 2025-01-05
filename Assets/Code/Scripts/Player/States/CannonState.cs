using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.XR;

public class CannonState : PlayerState
{
    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        
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



    public override void InteractAction() { /*No puedes interactuar*/ }
    public override void UseAction()
    {
        //Aqui deberias tener la caña asi que puedes usarla
        controller.Use();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        //Te pueden golpear y hacer daño
    }

    private void CannonMovement()
    {
        float forwardInput = controller.movementInput.y;
        float rotationInput = controller.movementInput.x;

        Vector3 moveDirection = controller.transform.forward * forwardInput;
        controller.rb.AddForce(moveDirection * controller.acceleration, ForceMode.Acceleration);

        if (rotationInput != 0)
        {
            float rotationAmount = rotationInput * controller.cannonRotationSpeed * Time.deltaTime;
            controller.transform.Rotate(0, rotationAmount, 0); // Rotate around the Y axis
        }

        if (controller.rb.linearVelocity.magnitude > controller.cannonMovementSpeed)
        {
            controller.rb.linearVelocity = new Vector3(
                controller.rb.linearVelocity.normalized.x * controller.cannonMovementSpeed,
                controller.rb.linearVelocity.y,
                controller.rb.linearVelocity.normalized.z * controller.cannonMovementSpeed
                );
        }
    }
}
