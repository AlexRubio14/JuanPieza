using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerInput playerInput;

    public Action<Vector2> OnMoveAction;
    public Action OnInteractAction;
    public Action OnStopInteractAction;
    public Action OnUseAction;
    public Action OnStopUseAction;
    public Action OnPushAction;
    public Action OnRollAction;
    public Action OnThrowAction;
    public Action OnDanceAction;
    public Action OnGrabAction;
    public Action OnReleaseAction;
    
    public Action<Vector2> OnWeaponRotateAction;
    public int playerReference { get;  set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void ReadMovement(InputAction.CallbackContext obj)
    {
        if (OnMoveAction != null)
            OnMoveAction(obj.ReadValue<Vector2>());
    }
    public void InteractAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnInteractAction != null)
                OnInteractAction();
        else if (obj.canceled && OnStopInteractAction != null)
                OnStopInteractAction();
    }
    public void UseAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnUseAction != null)
                OnUseAction(); 
        else if (obj.canceled && OnStopUseAction != null)
                OnStopUseAction();
    }
    public void RollAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnRollAction != null)
                OnRollAction();
    }
    public void ThrowAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnThrowAction != null)
                OnThrowAction();
    }
    public void DanceAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnDanceAction != null)
            OnDanceAction();
    }
    public void GrabAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnGrabAction != null)
                OnGrabAction();
        else if (obj.canceled && OnReleaseAction != null)
                OnReleaseAction();
    }
    public void PushAction(InputAction.CallbackContext obj)
    {
        if (obj.started && OnPushAction != null)
            OnPushAction();
    }

    public void RotateWeaponAction(InputAction.CallbackContext obj)
    {
        if (OnWeaponRotateAction != null)
            OnWeaponRotateAction(obj.ReadValue<Vector2>());
    }

    public void PauseAction(InputAction.CallbackContext obj)
    {
        if (obj.started)
            PauseManager.Instance.TogglePause();
    }
}
