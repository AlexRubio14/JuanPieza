using AYellowpaper.SerializedCollections;
using System;
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
        INTERACT, 
        HOLD 
    }
    
    [field: SerializeField]
    public DeviceType deviceType {  get; private set; }

    [Space]
    [SerializeField, SerializedDictionary("Action", "Device Sprites")] private SerializedDictionary<ActionType, List<Sprite>> ActionSprites;
    [Space, SerializeField] private Canvas canvas;
    [SerializeField] private Image hintImage;
    [SerializeField] private float hintOffset;

    [Space]
    [Header("Progress Bar")]
    [SerializeField] private ProgressBarController progressBar;
    [SerializeField] private float progressBarOffset;
    
    public bool isInteracting;
    
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
        if (hintImage.gameObject.activeInHierarchy)
        {
            Vector3 hintPos = transform.position + new Vector3(0, hintOffset, 0);
            hintImage.transform.position = hintPos;

        }
        
        if(progressBar.gameObject.activeInHierarchy) 
        {
            Vector3 progressBarPos = transform.position + new Vector3(progressBarOffset, hintOffset, 0);
            progressBar.transform.position = progressBarPos;
        }
    }

    public void UpdateActionType(ActionType _action)
    {
        if (_action == ActionType.NONE)
        {
            //Ocultar la UI de inputs
            hintImage.gameObject.SetActive(false);
            return;
        }
        
        //Mostrar la UI de inputs
        hintImage.gameObject.SetActive(true);
        
        Sprite currentSprite = ActionSprites[_action][(int)deviceType];
        hintImage.sprite = currentSprite;
    }
}
