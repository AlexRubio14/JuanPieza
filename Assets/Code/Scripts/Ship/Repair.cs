using System.Collections.Generic;
using UnityEngine;

public class Repair : InteractableObject
{
    [Header("Item")]
    [SerializeField] private InteractableObject itemNeeded;

    [Header("Time")]
    [SerializeField] private float repairDuration;
    [SerializeField] private float repairSpeed;
    private float currentRepairTime;

    [Header("Player")]
    private List<(PlayerController, ObjectHolder)> players;

    [Header("Ship")]
    protected Ship ship;
    private float damageDeal;

    private void Start()
    {
        players = new List<(PlayerController, ObjectHolder)>();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (_objectHolder.GetInteractableObject() == itemNeeded)
        {
            players.Add((_objectHolder.GetComponentInParent<PlayerController>(), _objectHolder));
        }
    }

    private void Update()
    {
        RepairObjec();
    }

    private void RepairObjec()
    {
        if (players.Count > 0)
        {
            Debug.Log("entra");
            currentRepairTime += repairSpeed * players.Count * Time.deltaTime;
            if (currentRepairTime >= repairDuration)
            {
                FinishRepairing();
                ship.SetCurrentHealth(damageDeal);
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
        for(int i = players.Count - 1; i >= 0; i--) 
        {
            RepairEnded(players[i].Item2);
        }
    }

    protected virtual void RepairEnded(ObjectHolder _objectHolder)
    {

    }

    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
    public override void UseItem(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
}
