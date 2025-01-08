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

    [SerializeField] private Ship ship;

    private void Start()
    {
        players = new List<PlayerController>();
        currentTime = sailTimer;
    }

    private void Update()
    {
        if (timerIsActive) 
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("00");
            if (currentTime <= 0f)
            {
                timerIsActive = false;
                //zarpar
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
                timerText.gameObject.SetActive(true);
            }

            if (players.Count == PlayersManager.instance.GetPlayers().Count)
            {
                //zarpar
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
        timerText.gameObject.SetActive(false);
        timerIsActive = false;
    }
}
