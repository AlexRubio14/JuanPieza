using UnityEngine;

public class RepairNPC : InteractableObject
{
    [SerializeField] private int repairCost = 100;
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (MoneyManager.Instance.SpendMoney(repairCost))
            ShipsManager.instance.playerShip.SetMaxHealth();
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }
}
