using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    [Header("RepairItem")]
    [SerializeField] private ObjectSO repairItem;

    [Header("Time")]
    [SerializeField] private float repairDuration;
    private float currentRepairTime;

    [Header("Player")]
    private List<PlayerController> players = new List<PlayerController>();

    [Header("State")]
    [SerializeField] protected ObjectState state;

    private void Update()
    {
        RepairObject();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if(state.GetIsBroken() && CanInteract(_objectHolder))
            players.Add(_objectHolder.GetComponentInParent<PlayerController>());
    }
    public override void StopInteract(ObjectHolder _objectHolder)
    {
        players.Remove(_objectHolder.GetComponentInParent<PlayerController>());
    }
    public override void UseItem(ObjectHolder _objectHolder)
    {

    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject || handObject && handObject.objectSO == repairItem;
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
        {
            return false;
        }

        return players.Contains(_playerController);
    }

    protected virtual void RepairEnded(ObjectHolder _objectHolder)
    {

    }

}
