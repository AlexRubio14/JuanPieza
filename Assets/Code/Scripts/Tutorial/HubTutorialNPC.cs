using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Cargar Actions
        hideArrowsAction += HideArrow;
        showBAAction += ShowBoardArrow;
        showMAAction += ShowMapArrow;
        waitCIAction += SetMenuInputMap;
        showSQAAction += ShowSelectQuestArrow;

        //Añadir actions al dialogueController
        DialogueController.instance.AddAction("D.HT.HA", hideArrowsAction);
        DialogueController.instance.AddAction("D.HT.SBA", showBAAction);
        DialogueController.instance.AddAction("D.HT.SMA", showMAAction);
        DialogueController.instance.AddAction("D.HT.WCI", waitCIAction);
        DialogueController.instance.AddAction("D.HT.SSQA", showSQAAction);

        HideArrow();

        boardObject = FindAnyObjectByType<QuestBoardObject>();
        boardObject.GetQuestCanvas().SetActive(true);
        boardCanvas = boardObject.GetComponentInChildren<QuestBoard>();
        boardObject.GetQuestCanvas().SetActive(false);
        mapTutorial = false;
        selectQuestTutorial = false;
    }

    private void FixedUpdate()
    {
        foreach (GameObject obj in arrows)
            obj.transform.forward = -Camera.main.transform.forward;


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
        arrows[0].transform.SetParent(boardCanvas.GetQuestIcons()[0].transform);
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



}
