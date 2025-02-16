using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    public DialogueData dialogue;
    [SerializeField]
    private float timeBetweenLetters;
    private int letterIndex;
    private bool showingText = false;
    private bool displayingDialogue = false;
    private Dictionary<string, Action> actionList;

    [Space, SerializeField]
    private GameObject dialogueObject;
    private TextMeshProUGUI dialogueText;


    [Space, Header("Audio"), SerializeField]
    private AudioClip letterSpawnSound;
    [SerializeField]
    private AudioClip clickSound;
    private int dialogueSoundIndex;

    private void Awake()
    {
        dialogueText = dialogueObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void StartDialogue()
    {
        if (dialogue.sequence.Count == 0)
        {
            FinishDialogue();
            return;
        }

        //Empezar con el dialogo
        dialogueObject.SetActive(true);
        letterIndex = 0;
        dialogueText.maxVisibleCharacters = letterIndex;
        showingText = true;

    }

    private void InputPressed(InputAction.CallbackContext obj)
    {
        if (showingText)
        {
            if (displayingDialogue)
                DisplayAllLetters();
            else
                ReadDialogueType(dialogue.GetNextDialogue());

            //AudioManager.instance.Play2dOneShotSound(clickSound, "Button");
        }
    }

    private void ReadDialogueType(DialogueData.Dialogue _dialogue)
    {
        switch (_dialogue.type)
        {
            case DialogueData.DialogueType.DIALOGUE:
                DisplayNextDialogue(_dialogue.dialogue);
                break;
            case DialogueData.DialogueType.ACTION:
                DoAction(_dialogue.actionId);
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
        showingText = false;
        displayingDialogue = false;
        gameObject.SetActive(false);
    }

    private void DoAction(string _actionId)
    {
        if (actionList.ContainsKey(_actionId) && actionList[_actionId] != null)
            actionList[_actionId]();
    }

    private void DisplayLetter()
    {
        if (displayingDialogue)
        {

            if (letterIndex >= dialogue.GetCurrentDialogue().dialogue.Length)
            {
                displayingDialogue = false;
            }
            dialogueText.maxVisibleCharacters = letterIndex;
            letterIndex++;
            Invoke("DisplayLetter", timeBetweenLetters);
            if (dialogueSoundIndex % 4 == 0)
                //AudioManager.instance.Play2dOneShotSound(letterSpawnSound, "SFX", 0.35f, 2f, 2.5f);

            dialogueSoundIndex++;
        }
    }
    private void DisplayAllLetters()
    {
        displayingDialogue = false;
        dialogueText.maxVisibleCharacters = dialogue.GetCurrentDialogue().dialogue.Length;
    }

    public void AddAction(string _actionId, Action _action)
    {

    }
}
