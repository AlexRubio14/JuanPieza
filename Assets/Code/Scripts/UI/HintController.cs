using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    public enum DeviceType { KEYBOARD = 0, GAMEPAD = 1}
    public enum ActionType { 
        NONE, 
        USE, 
        HOLD_USE,
        INTERACT, 
        HOLD_INTERACT
    }
    
    [field: SerializeField]
    public DeviceType deviceType {  get; private set; }

    [Space]
    [SerializeField, SerializedDictionary("Action", "Device Sprites")] 
    private SerializedDictionary<ActionType, List<Sprite>> ActionSprites;
    [Space, SerializeField] private Canvas canvas;
    [SerializeField] private Image hintRightImage;
    [SerializeField] private Image hintLeftImage;

    [SerializeField] private float hintOffset;
    
    private void Start()
    {
        canvas.worldCamera = Camera.main;
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
        if (hintRightImage.gameObject.activeInHierarchy)
        {
            Vector3 hintPos = transform.position + new Vector3(hintOffset, 0, 0);
            hintRightImage.transform.position = hintPos;
        }

    }

    public void UpdateActionType(ActionType _action)
    {
        if (_action == ActionType.NONE)
        {
            //Ocultar la UI de inputs
            hintRightImage.gameObject.SetActive(false);
            hintLeftImage.gameObject.SetActive(false);
            return;
        }

        if (_action == ActionType.USE)
        {
            hintRightImage.gameObject.SetActive(true);

            Sprite currentSprite = ActionSprites[_action][(int)deviceType];
            hintRightImage.sprite = currentSprite;
        }
        if (_action == ActionType.INTERACT)
        {
            hintLeftImage.gameObject.SetActive(true);

            Sprite currentSprite = ActionSprites[_action][(int)deviceType];
            hintLeftImage.sprite = currentSprite;
        }
    }
}
