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
    [SerializeField] private ProgressBar progressBar;
    
    public bool isInteracting;
    
    private void Start()
    {
        canvas.worldCamera = Camera.main;

        PlayerInput input = PlayersManager.instance.players[GetComponent<PlayerController>().playerInput.playerReference].Item1; 
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
        Vector3 playerPos = transform.position + new Vector3(0, hintOffset, 0);
        hintImage.transform.position = playerPos;
        progressBar.transform.position = playerPos;
    }

    public void UpdateActionType(ActionType _action)
    {
        if (_action == ActionType.NONE)
        {
            //Ocultar la UI de inputs
            hintImage.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
            progressBar.SetCurrentValue(0);
            return;
        }

        if (_action == ActionType.HOLD)
        {
            hintImage.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
            return;
        }
        
        //Mostrar la UI de inputs
        hintImage.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(false);
        
        Sprite currentSprite = ActionSprites[_action][(int)deviceType];
        hintImage.sprite = currentSprite;
    }

    public void SetProgressBar(float max, float current)
    {
        progressBar.SetMaxValue(max);
        progressBar.SetCurrentValue(current);
    }
}
