public class RepairObject : Repair
{
    protected override void RepairEnded()
    {
        base.RepairEnded();
        state.SetIsBroke(false);
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (handObject && handObject.objectSO == repairItem)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.HOLD_USE, "repair")

            };
        else if (!handObject)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
