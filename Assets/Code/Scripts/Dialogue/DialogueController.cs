using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance;

    [SerializeField]
    private InputActionReference dialogueAction;

    [Space, SerializeField]
    public DialogueData dialogue;
    [SerializeField]
    private float timeBetweenLetters;
    private int letterIndex;
    private bool showingText = false;
    private bool displayingDialogue = false;
    private Dictionary<string, Action> actionList = new Dictionary<string, Action>();
    public int sequenceIndex = -1;


    [Space, SerializeField]
    private GameObject dialogueObject;
    private TextMeshProUGUI dialogueText;


    [Space, Header("Audio"), SerializeField]
    private AudioClip letterSpawnSound;
    [SerializeField]
    private AudioClip clickSound;
    private int dialogueSoundIndex;

    [Space, SerializedDictionary("UI Image", "Input Sprites")]
    public SerializedDictionary<Image, Sprite[]> actionsSprites;

    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(instance.gameObject);

        instance = this;

        dialogueText = dialogueObject.GetComponentInChildren<TextMeshProUGUI>();
        sequenceIndex = -1;
        showingText = false;
        displayingDialogue = false;
        FinishDialogue();
    }
    private void OnEnable()
    {
        dialogueAction.action.started += InputPressed;

        StartCoroutine(UpdateInputImages());
    }
    private void OnDisable()
    {
        dialogueAction.action.started -= InputPressed;
    }
    
    public void StartDialogue(DialogueData _dialogueData)
    {
        dialogue = _dialogueData;
        if (dialogue.sequence.Count == 0)
        {
            sequenceIndex = -1;
            showingText = false;
            displayingDialogue = false;
            dialogueObject.SetActive(false);
            return;
        }

        //Empezar con el dialogo
        dialogueObject.SetActive(true);
        sequenceIndex = -1;
        letterIndex = 0;
        dialogueText.maxVisibleCharacters = letterIndex;
        showingText = true;
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Dialogue");

        StartCoroutine(UpdateInputImages());
        ReadDialogueType();
    }

    private void InputPressed(InputAction.CallbackContext obj)
    {
        if (showingText)
        {
            if (displayingDialogue)
                DisplayAllLetters();
            else
                ReadDialogueType();
        }
    }

    private void ReadDialogueType()
    {
        sequenceIndex++;
        DialogueData.Dialogue currentDialogue = dialogue.GetDialogue(sequenceIndex);

        switch (currentDialogue.type)
        {
            case DialogueData.DialogueType.START:
                ReadDialogueType();
                break;
            case DialogueData.DialogueType.DIALOGUE:
                DisplayNextDialogue(currentDialogue.dialogue);
                AudioManager.instance.Play2dOneShotSound(clickSound, "Button", 1.5f, 0.95f, 1.05f);
                break;
            case DialogueData.DialogueType.ACTION:
                DoAction(currentDialogue.actionId);
                break;
            case DialogueData.DialogueType.END:
                FinishDialogue();
                break;
            default:
                break;
        }
    }

    private void DisplayNextDialogue(string _dialogueText)
    {
        //Si aun no se ha acabado el dialogo
        displayingDialogue = true;
        letterIndex = 0;
        dialogueText.text = _dialogueText;
        dialogueText.maxVisibleCharacters = letterIndex;

        Invoke("DisplayLetter", timeBetweenLetters);
    }

    private void FinishDialogue()
    {
        //Si no hay mas dialogos
        sequenceIndex = -1;
        showingText = false;
        displayingDialogue = false;
        foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
            item.Item1.SwitchCurrentActionMap("Gameplay");
        dialogueObject.SetActive(false);
    }

    private void DoAction(string _actionId)
    {
        if (actionList.ContainsKey(_actionId) && actionList[_actionId] != null)
            actionList[_actionId]();

        ReadDialogueType();
    }

    private void DisplayLetter()
    {
        if (displayingDialogue)
        {

            if (letterIndex >= dialogue.GetDialogue(sequenceIndex).dialogue.Length)
            {
                displayingDialogue = false;
            }
            dialogueText.maxVisibleCharacters = letterIndex;
            letterIndex++;
            Invoke("DisplayLetter", timeBetweenLetters);
            if (dialogueSoundIndex % 4 == 0)
                AudioManager.instance.Play2dOneShotSound(letterSpawnSound, "SFX", 0.35f, 2f, 2.5f);

            dialogueSoundIndex++;
        }
    }
    private void DisplayAllLetters()
    {
        displayingDialogue = false;
        dialogueText.maxVisibleCharacters = dialogue.GetDialogue(sequenceIndex).dialogue.Length;
    }

    public void AddAction(string _actionId, Action _action)
    {
        if(!actionList.ContainsKey(_actionId))
            actionList.Add(_actionId, _action);
    }



    private IEnumerator UpdateInputImages()
    {
        yield return new WaitForEndOfFrame();
        HintController.DeviceType device = PlayersManager.instance.ingamePlayers.Count > 0 
            ? PlayersManager.instance.ingamePlayers[0].hintController.deviceType 
            : HintController.DeviceType.KEYBOARD;
        foreach (KeyValuePair<Image, Sprite[]> item in actionsSprites)
        {
            item.Key.sprite = item.Value[(int)device];
        }
    }
}
