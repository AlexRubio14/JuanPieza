using UnityEngine;

public class RepairHole : Repair
{
    private Ship ship;
    private float damageDeal;
    [SerializeField] private bool hasToRecoverHP;

    protected override void RepairEnded()
    {
        base.RepairEnded();
        if(hasToRecoverHP)
            ship.SetCurrentHealth(damageDeal);
        Destroy(gameObject);
    }
    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (handObject && handObject.objectSO == repairItem)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.HOLD_USE, "repair_hole")
            };


        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
    public void SetDamageDeal(float _damageDeal) 
    {
        damageDeal -= _damageDeal;
    }
    public float GetDamageDeal()
    {
        return damageDeal;
    }
}
