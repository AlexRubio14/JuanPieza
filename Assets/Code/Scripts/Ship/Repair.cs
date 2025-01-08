using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    [Space, Header("Repair Item")]
    [SerializeField] private ObjectSO repairItem;
    [SerializeField] protected ObjectState state;
    [SerializeField] private float repairDuration;
    private float currentRepairTime;
    private List<PlayerController> players = new List<PlayerController>();


    private void Update()
    {
        RepairObject();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if(state.GetIsBroken() && CanInteract(_objectHolder))
        {
            PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
            playerCont.animator.SetBool("Interacting", true);
            playerCont.progressBar.EnableProgressBar(true);
            players.Add(playerCont);
        }
    }
    public override void StopInteract(ObjectHolder _objectHolder)
    {
        PlayerController playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        playerCont.animator.SetBool("Interacting", false);
        playerCont.progressBar.EnableProgressBar(false);
        players.Remove(playerCont);
    }
    public override void UseItem(ObjectHolder _objectHolder)
    {

    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return handObject && handObject.objectSO == repairItem;
    }

    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        
        if (handObject && handObject.objectSO == repairItem)
        {
            return HintController.ActionType.HOLD;
        }

        return HintController.ActionType.NONE;
    }

    private void RepairObject()
    {
        if (players.Count > 0)
        {
            currentRepairTime += players.Count * Time.deltaTime;
            if (currentRepairTime >= repairDuration)
            {
                FinishRepairing();
                currentRepairTime = 0;
            }

            foreach (PlayerController player in players)
            {
                player.progressBar.SetProgress(currentRepairTime, repairDuration);
            }
        }
        else
        {
            currentRepairTime = 0;
        }
    }
    private void FinishRepairing()
    {
        for (int i = players.Count - 1; i >= 0; i--)
        {
            RepairEnded(players[i].objectHolder);
            StopInteract(players[i].objectHolder);
        }
    }

    public bool IsPlayerReparing(PlayerController _playerController)
    {
        if(players.Count <= 0) 
            return false;

        return players.Contains(_playerController);
    }
    public ObjectState GetObjectState() { return state; }

    protected virtual void RepairEnded(ObjectHolder _objectHolder)
    {

    }

}
