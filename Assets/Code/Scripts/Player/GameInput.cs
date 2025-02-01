using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public Action<Vector2> OnMoveAction;
    public Action OnInteractAction;
    public Action OnStopInteractAction;
    public Action OnUseAction;
    public Action OnPushAction;
    public Action OnRollAction;
    public Action OnThrowAction;

    public Action<float> OnWeaponTiltAction;
    public int playerReference { get;  set; }


    public void ReadMovement(InputAction.CallbackContext obj)
    {
        if (OnMoveAction != null)
            OnMoveAction(obj.ReadValue<Vector2>());
    }
    public void InteractAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (OnInteractAction != null)
                OnInteractAction();
        }
        else if (obj.canceled)
        {
            if(OnStopInteractAction != null)
                OnStopInteractAction();
        }
    }
    public void UseAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (OnUseAction != null)
                OnUseAction();
        }
    }
    public void RollAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (OnRollAction != null)
                OnRollAction();
        }
    }
    public void ThrowAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            if (OnThrowAction != null)
                OnThrowAction();
        }
    }

    public void TiltCannon(InputAction.CallbackContext obj)
    {
        if (OnWeaponTiltAction != null)
            OnWeaponTiltAction(obj.ReadValue<float>());
    }
}
