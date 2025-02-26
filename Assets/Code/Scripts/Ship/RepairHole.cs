using UnityEngine;

public class RepairHole : Repair
{
    private Ship ship;
    private float damageDeal;
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        _objectHolder.hintController.UpdateActionType(new HintController.Hint[] { new HintController.Hint(HintController.ActionType.NONE, "") });
        ship.SetCurrentHealth(damageDeal);
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        Destroy(currentObject.gameObject);
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
}
