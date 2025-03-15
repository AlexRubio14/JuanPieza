using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HubTutorialNPC : MonoBehaviour
{
    [SerializeField]
    private DialogueData starterDialogue;
    [SerializeField]
    private DialogueData mapTutorialDialogue;
    private bool mapTutorial;
    [SerializeField]
    private DialogueData selectQuestDialogue;
    private bool selectQuestTutorial;

    private QuestBoardObject boardObject;
    private QuestBoard boardCanvas;


    [Space, Header("Camera Changes"), SerializeField]
    private Camera secondaryCamera;
    private Camera mainCamera;
    [SerializeField]
    private Vector3 playerCameraPos;
    [SerializeField]
    private Quaternion playerCameraRot;
    [SerializeField]
    private Vector3 mentorCameraPos;
    [SerializeField]
    private Quaternion mentorCameraRot;
    [SerializeField]
    private RuntimeAnimatorController tutorialAnimator;
    [SerializeField]
    private RuntimeAnimatorController baseAnimator;


    private Action lookPlayerAction;
    private Action lookMentorAction;
    private Action excitedMentorAction;
    private Action sadMentorAction;
    private Action handUpMentorAction;
    private Action idleMentorAction;
    private Action resetPlayerAction;
    

    [Space, Header("Show Arrow"), SerializeField]
    private GameObject[] arrows;
    [SerializeField]
    private GameObject arrowCanvas;
    [SerializeField]
    private Vector3 arrowOffset;

    private Action hideArrowsAction;
    private Action showBAAction;
    private Action showMAAction;
    private Action waitCIAction;
    private Action showSQAAction;
    private Action disableQuestIconAction;
    private Action enableQuestIconAction;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //Cargar Actions Camera
        lookPlayerAction += LookPlayerAction;
        lookMentorAction += LookMentorAction;
        excitedMentorAction += ExcitedMentorAction;
        sadMentorAction += SadMentorAction;
        handUpMentorAction += HandUpMentorAction;
        idleMentorAction += IdleMentorAction;
        resetPlayerAction += ResetPlayerAction;
        


        //Añadir actions al dialogueController
        DialogueController.instance.AddAction("D.HT.LP", lookPlayerAction);
        DialogueController.instance.AddAction("D.HT.LM", lookMentorAction);
        DialogueController.instance.AddAction("D.HT.EM", excitedMentorAction);
        DialogueController.instance.AddAction("D.HT.SM", sadMentorAction);
        DialogueController.instance.AddAction("D.HT.HUM", handUpMentorAction);
        DialogueController.instance.AddAction("D.HT.IM", idleMentorAction);
        DialogueController.instance.AddAction("D.HT.RP", resetPlayerAction);


        //Cargar Actions Arrows
        hideArrowsAction += HideArrow;
        showBAAction += ShowBoardArrow;
        showMAAction += ShowMapArrow;
        waitCIAction += SetMenuInputMap;
        showSQAAction += ShowSelectQuestArrow;
        disableQuestIconAction += DisableQuestIcons;
        enableQuestIconAction += EnableQuestIcons;

        //Añadir actions al dialogueController
        DialogueController.instance.AddAction("D.HT.HA", hideArrowsAction);
        DialogueController.instance.AddAction("D.HT.SBA", showBAAction);
        DialogueController.instance.AddAction("D.HT.SMA", showMAAction);
        DialogueController.instance.AddAction("D.HT.WCI", waitCIAction);
        DialogueController.instance.AddAction("D.HT.SSQA", showSQAAction);
        DialogueController.instance.AddAction("D.HT.DQI", disableQuestIconAction);
        DialogueController.instance.AddAction("D.HT.EQI", enableQuestIconAction);

        HideArrow();

        boardObject = FindAnyObjectByType<QuestBoardObject>();
        boardObject.GetQuestCanvas().SetActive(true);
        boardCanvas = boardObject.GetComponentInChildren<QuestBoard>();
        boardObject.GetQuestCanvas().SetActive(false);
        mapTutorial = false;
        selectQuestTutorial = false;

        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        foreach (GameObject obj in arrows)
            obj.transform.forward = -mainCamera.transform.forward;


        if(!mapTutorial && boardCanvas.gameObject.activeInHierarchy)
        {
            mapTutorial = true;
            DialogueController.instance.StartDialogue(mapTutorialDialogue);
        }
        else if (!selectQuestTutorial && boardCanvas.GetInformationCanvas().activeInHierarchy)
        {
            selectQuestTutorial = true;
            DialogueController.instance.StartDialogue(selectQuestDialogue);
        }
    }

    public void PlayStarterDialogue()
    {
        DialogueController.instance.StartDialogue(starterDialogue);
    }
    private void SetMenuInputMap()
    {
        StartCoroutine(WaitToChangeInputMap());
        IEnumerator WaitToChangeInputMap()
        {
            yield return new WaitForSeconds(1f);
            foreach ((PlayerInput, SinglePlayerController) item in PlayersManager.instance.players)
                item.Item1.SwitchCurrentActionMap("MapMenu");
        }
        
    }
    private void HideArrow()
    {
        foreach (GameObject obj in arrows)
        {
            obj.transform.SetParent(arrowCanvas.transform);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
        }
    }
    private void ShowBoardArrow()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.position = boardObject.transform.position + arrowOffset;
    }
    private void ShowMapArrow()
    {
        arrows[0].SetActive(true);
        arrows[0].transform.SetParent(boardCanvas.transform);
        arrows[0].transform.localScale = Vector3.one * 100;
        arrows[0].transform.position = boardCanvas.GetQuestIcons()[0].transform.position + arrowOffset * 50;
    }
    private void ShowSelectQuestArrow()
    {
        arrows[1].SetActive(true);
        arrows[1].transform.SetParent(boardCanvas.GetInformationCanvas().transform);
        arrows[1].transform.localScale = Vector3.one * 100;
        arrows[1].transform.position = boardCanvas.GetInformationCanvas().GetComponentInChildren<AcceptButton>().transform.position + arrowOffset * 50;
    }

    private void LookPlayerAction()
    {
        mainCamera.gameObject.SetActive(false);
        secondaryCamera.gameObject.SetActive(true);
        secondaryCamera.transform.position = playerCameraPos;
        secondaryCamera.transform.rotation = playerCameraRot;

        PlayerController currentPlayer = PlayersManager.instance.ingamePlayers[0];

        currentPlayer.transform.forward = (
            new Vector3(transform.position.x, 0, transform.position.z) 
            - 
            new Vector3(currentPlayer.transform.position.x, 0, currentPlayer.transform.position.z)
            ).normalized;

        currentPlayer.animator.runtimeAnimatorController = tutorialAnimator;
        currentPlayer.animator.SetTrigger("Fear");
    }
    private void LookMentorAction()
    {
        secondaryCamera.transform.position = mentorCameraPos;
        secondaryCamera.transform.rotation = mentorCameraRot;

        animator.SetTrigger("AimHand");
    }
    private void ExcitedMentorAction()
    {
        animator.SetTrigger("Celebrate");
    }
    private void SadMentorAction()
    {
        animator.SetTrigger("Sad");
        animator.SetBool("isSad", true);
    }
    private void HandUpMentorAction()
    {
        animator.SetTrigger("HandUp");
        animator.SetBool("isHandUp", true);
        StartCoroutine(WaitToChangeIsSad());

        IEnumerator WaitToChangeIsSad()
        {
            yield return new WaitForEndOfFrame();
            animator.SetBool("isSad", false);
        }

    }
    private void IdleMentorAction()
    {
        animator.ResetTrigger("HandUp");
        animator.SetBool("isHandUp", false);
    }
    private void ResetPlayerAction()
    {
        mainCamera.gameObject.SetActive(true);
        secondaryCamera.gameObject.SetActive(false);

        PlayersManager.instance.ingamePlayers[0].animator.runtimeAnimatorController = baseAnimator;
    }

    private void DisableQuestIcons()
    {
        foreach (QuestIcon item in boardCanvas.GetQuestIcons())
            item.GetComponent<Button>().interactable = false;
    }
    private void EnableQuestIcons()
    {
        StartCoroutine(WaitEndOfFrame());

        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            foreach (QuestIcon item in boardCanvas.GetQuestIcons())
                item.GetComponent<Button>().interactable = true;

            boardCanvas.GetQuestIcons()[0].GetComponent<Button>().Select();
        }
    }
}
