using System;
using UnityEngine;
using UnityEngine.AI;

public class HubTutorialNPC : MonoBehaviour
{
    [SerializeField]
    private DialogueData starterDialogue;

    private NavMeshAgent agent;
    private Animator animator;

    [Space, Header("Show Arrow"), SerializeField]
    private GameObject[] arrows;
    [SerializeField]
    private Vector3 arrowOffset;

    private Action hideArrowsAction;
    private Action showMAAction;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //Cargar Actions
        hideArrowsAction += HideArrow;
        showMAAction += ShowMapArrow;

        //Añadir actions al dialogueController
        DialogueController.instance.AddAction("D.HT.HA", hideArrowsAction);
        DialogueController.instance.AddAction("D.HT.SMA", showMAAction);

        HideArrow();
    }

    private void FixedUpdate()
    {
        foreach (GameObject obj in arrows)
            obj.transform.forward = -Camera.main.transform.forward;
    }

    public void PlayStarterDialogue()
    {
        DialogueController.instance.StartDialogue(starterDialogue);
    }


    private void HideArrow()
    {
        foreach (GameObject obj in arrows)
            obj.SetActive(false);
    }
    
    private void ShowMapArrow()
    {
        GameObject board = GameObject.Find("QuestBoard");
        arrows[0].SetActive(true);
        arrows[0].transform.position = board.transform.position + arrowOffset; //GUARRADA HISTORICA ALERTA!!!!!!!!!!!!!!!!!!!!!!!!
    }





}
