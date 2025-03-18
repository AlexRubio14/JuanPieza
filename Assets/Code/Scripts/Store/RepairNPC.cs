using UnityEngine;

public class RepairNPC : InteractableObject
{
    [SerializeField] private int repairCost = 100;
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (MoneyManager.Instance.SpendMoney(repairCost))
            ShipsManager.instance.playerShip.SetMaxHealth();
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }
}
