using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShippingSail : InteractableObject
{
    private List<PlayerController> players;

    [Space, SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float sailTimer;
    private float currentTime;
    private bool timerIsActive;

    [SerializeField] private AllyShip ship;
    [SerializeField] private GameObject[] bridge;

    private bool canInteract;
    private bool isMainShip;

    protected override void Start()
    {
        base.Start();
        if(!isMainShip)
            ActiveBridge(true);
        players = new List<PlayerController>();
        VotationCanvasManager.Instance.SetSailTimer(false);
        currentTime = sailTimer;
    }

    private void Update()
    {
        if (timerIsActive) 
        {
            currentTime -= Time.deltaTime;
            VotationCanvasManager.Instance.SetSailtText(currentTime);
            if (currentTime <= 0f)
            {
                timerIsActive = false;
                VotationCanvasManager.Instance.SetSailTimer(false);
                ActiveBridge(false);
                ship.StartVotation();
            }
        }
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (ship.CheckOverweight() || !canInteract)
            return;

        PlayerController interactingPlayer = _objectHolder.GetComponentInParent<PlayerController>();
        if (players.Contains(interactingPlayer))
        {
            players.Remove(interactingPlayer);
            if (players.Count <= 0)
            {
                ResetTimer();
            }
        }
        else
        {
            players.Add(interactingPlayer);

            if (!timerIsActive)
            {
                timerIsActive = true;
                VotationCanvasManager.Instance.SetSailTimer(true);
            }

            if (players.Count == PlayersManager.instance.GetPlayers().Count)
            {
                VotationCanvasManager.Instance.SetSailTimer(false);
                ship.StartVotation();
            }
        }
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }


    private void ResetTimer()
    {
        currentTime = sailTimer;
        VotationCanvasManager.Instance.SetSailTimer(false);
        timerIsActive = false;
    }

    public void ActiveBridge(bool state)
    {
        foreach (GameObject _bridge in bridge)
            _bridge.SetActive(state);
        canInteract = state;
    }

    public void SetIsMainShip(bool state)
    {
        isMainShip = state;
    }

    public bool GetIsBridgeActive()
    {
        return bridge[0].activeSelf;
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHandInteractableObject())
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "sail"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
