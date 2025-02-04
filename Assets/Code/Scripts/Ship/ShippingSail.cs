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

    protected override void Start()
    {
        base.Start();
        players = new List<PlayerController>();
        VotationCanvasManager.Instance.SetSailTimer(false);
        currentTime = sailTimer;
        foreach (GameObject _bridge in bridge)
            _bridge.SetActive(false);
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
                foreach(GameObject _bridge in bridge) 
                    _bridge.SetActive(false);
                ship.StartVotation();
            }
        }
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (ship.CheckOverweight())
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

    public override void UseItem(ObjectHolder _objectHolder)
    {
        
    }


    private void ResetTimer()
    {
        currentTime = sailTimer;
        VotationCanvasManager.Instance.SetSailTimer(false);
        timerIsActive = false;
    }

    public void ActiveBridge()
    {
        foreach (GameObject _bridge in bridge)
            _bridge.SetActive(true);
    }
}
