using UnityEngine;
using UnityEngine.InputSystem;


public class HintController : MonoBehaviour
{
    public enum DeviceType { KEYBOARD = 0, GAMEPAD = 1}
    public enum ActionType { 
        GRAB,
        USE, 
        HOLD_USE,
        INTERACT, 
        HOLD_INTERACT
    }
    
    [field: SerializeField]
    public DeviceType deviceType {  get; private set; }

    private InteractableObject lastInteractObject;

    private ObjectHolder objectHolder;

    private Hint playerHint;

    private void Awake()
    {
        playerHint = GetComponent<Hint>();
        objectHolder = GetComponentInChildren<ObjectHolder>();
    }
    private void Start()
    {
        PlayerController playerCont = GetComponent<PlayerController>();
        PlayerInput input = PlayersManager.instance.players[playerCont.playerInput.playerReference].Item1; 
        InputDevice device = input.devices[0];  // En este caso, tomamos el primer dispositivo de la lista

        if (device is Gamepad)
            deviceType = DeviceType.GAMEPAD;
        else if (device is Keyboard)
            deviceType = DeviceType.KEYBOARD;
        else
            Debug.Log($"Input no reconocido, el dispositivo es: {device.displayName}");

    }

    private void Update()
    {
        InteractableObject handObject = objectHolder.GetHandInteractableObject();
        InteractableObject nearObject = objectHolder.GetNearestInteractableObject();

        CheckObjectToGrab(handObject, nearObject);
        CheckObjectToUse(handObject);
        CheckObjetToInteract(handObject, nearObject);
    }

    private void CheckObjectToGrab(InteractableObject _handObject, InteractableObject _nearestObject)
    {
        if (!_handObject && !_nearestObject)
        {
            playerHint.DisableHint(ActionType.GRAB);
            return;
        }

        if (_handObject)
            playerHint.EnableHint(ActionType.GRAB, deviceType);
        else if (_nearestObject && _nearestObject.CanGrab(objectHolder))
            _nearestObject.hint.EnableHint(ActionType.GRAB, deviceType);


    }
    private void CheckObjectToUse(InteractableObject _handObject)
    {
        if (!_handObject || !_handObject.canUse)
        {
            playerHint.DisableHint(ActionType.USE);
            playerHint.DisableHint(ActionType.HOLD_USE);
            return;
        }
        //Mostrar el input hint en la cabeza del player
        playerHint.EnableHint(_handObject.hint.useType, deviceType);
    }
    private void CheckObjetToInteract(InteractableObject _handObject, InteractableObject _nearestObject)
    {
        /*
         * Esta comprobacion se hace por si dos players estan mirando a un objeto y uno puede hacer una accion que el otro no.
         * Asi aunque el que puede no la mire el objeto el hint no se quede de forma permanente
         */

        if(lastInteractObject != _nearestObject && lastInteractObject)
        {
            if (!lastInteractObject.hint.SomePlayerCanInteract(lastInteractObject))
            {
                lastInteractObject.hint.DisableHint(lastInteractObject.hint.interactType);
            }
        }

        lastInteractObject = _nearestObject;

        if (!_nearestObject && _handObject is not Weapon)
            return;

        if (_nearestObject && !_nearestObject.CanInteract(objectHolder))
        {
            if (_nearestObject.hint is RepairItemHint && ((_nearestObject as Repair).GetObjectState().GetIsBroken() || _nearestObject is Weapon && (_nearestObject as Weapon).GetFreeze()))
            {
                Repair repairItem = _nearestObject as Repair;
                RepairItemHint repairItemHint = _nearestObject.hint as RepairItemHint;

                if (repairItem.CanRepair(objectHolder) && repairItem.GetObjectState().GetIsBroken())
                {
                    //Mostrar progress bar
                    repairItemHint.progressBar.gameObject.SetActive(true);
                }
                else
                    repairItemHint.ShowRepairSprite();
            }
            return;
        }

        //En caso de tener un arma en la mano y que se pueda usar mostraremos el input
        if (_handObject && _handObject is Weapon && _handObject.CanInteract(objectHolder))
        {
            _handObject.hint.EnableHint(_handObject.hint.interactType, deviceType);
            return;
        }

        //Mostrar el hint de interactuar en el objeto mas cercano
        if(_nearestObject)
            _nearestObject.hint.EnableHint(_nearestObject.hint.interactType, deviceType);
    }   
}
