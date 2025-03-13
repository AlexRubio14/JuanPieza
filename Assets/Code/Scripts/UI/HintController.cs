using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HintController : MonoBehaviour
{
    [SerializeField]
    private HintActionData.Language language;

    [SerializeField]
    private HintActionData[] hintActions;
    public enum DeviceType { KEYBOARD = 0, GAMEPAD = 1}
    public enum ActionType { 
        CANT_USE,
        USE, 
        HOLD_USE,
        CANT_INTERACT,
        INTERACT, 
        HOLD_INTERACT,
        NONE
    }
    
    [field: SerializeField]
    public DeviceType deviceType {  get; private set; }

    [Space]
    [SerializeField, SerializedDictionary("Action", "Device Sprites")] 
    private SerializedDictionary<ActionType, List<Sprite>> ActionSprites;
    [Space, SerializeField] private Canvas canvas;
    [SerializeField] private Image hintRightImage;
    [SerializeField] private Image hintLeftImage;
    private TextMeshProUGUI hintRightText;
    private TextMeshProUGUI hintLeftText;

    [SerializeField] private float hintOffset;

    public bool showingHints;

    public struct Hint
    {
        public Hint(ActionType _type, string _id)
        {
            hintType = _type;
            hintId = _id;
        }
        public ActionType hintType;
        public string hintId;
    }

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


        hintRightText = hintRightImage.GetComponentInChildren<TextMeshProUGUI>();
        hintLeftText = hintLeftImage.GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Update()
    {
        if (hintRightImage.gameObject.activeInHierarchy)
        {
            Vector3 hintPos = transform.position + new Vector3(hintOffset, 0, 0);
            hintRightImage.transform.position = hintPos;
            hintRightImage.transform.forward = Camera.main.transform.forward;
        }

        if (hintLeftImage.gameObject.activeInHierarchy)
        {
            Vector3 hintPos = transform.position + new Vector3(-hintOffset, 0, 0);
            hintLeftImage.transform.position = hintPos;
            hintLeftImage.transform.forward = Camera.main.transform.forward;
        }

    }

    private HintActionData GetDataById(string _stringId)
    {
        foreach (HintActionData item in hintActions)
        {
            if(item.HintId == _stringId)
                return item;
        }

        return null;
    }

    private void UpdateRightAction(Hint _action)
    {
        switch (_action.hintType)
        {
            case ActionType.CANT_INTERACT:
            case ActionType.NONE:
                hintRightImage.gameObject.SetActive(false);
                break;
            case ActionType.INTERACT:
            case ActionType.HOLD_INTERACT:
                hintRightImage.gameObject.SetActive(true);

                Sprite currentSprite = ActionSprites[_action.hintType][(int)deviceType];
                hintRightImage.sprite = currentSprite;
                hintRightText.text = GetDataById(_action.hintId).hintTexts[language];

                break;
            default:
                break;
        }


    }
    private void UpdateLeftAction(Hint _action)
    {
        switch (_action.hintType)
        {
            case ActionType.CANT_USE:
            case ActionType.NONE:
                hintLeftImage.gameObject.SetActive(false);
                break;
            case ActionType.USE:
            case ActionType.HOLD_USE:
                hintLeftImage.gameObject.SetActive(true);

                Sprite currentSprite = ActionSprites[_action.hintType][(int)deviceType];
                hintLeftImage.sprite = currentSprite;
                hintLeftText.text = GetDataById(_action.hintId).hintTexts[language];
                break;
            default:
                break;
        }

    }

    public void UpdateActionType(Hint[] _actions)
    {

        foreach (Hint item in _actions)
        {
            switch (item.hintType)
            {
                case ActionType.CANT_USE:
                case ActionType.USE:
                case ActionType.HOLD_USE:
                    UpdateLeftAction(item);
                    break;
                case ActionType.CANT_INTERACT:
                case ActionType.INTERACT:
                case ActionType.HOLD_INTERACT:
                    UpdateRightAction(item);
                    break;
                case ActionType.NONE:
                    UpdateRightAction(item);
                    UpdateLeftAction(item);
                    showingHints = false;
                    break;
                default:
                    break;
            }
        }

        showingHints = hintRightImage.gameObject.activeInHierarchy || hintLeftImage.gameObject.activeInHierarchy;

    }
}
