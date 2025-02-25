public class RepairObject : Repair
{
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        state.SetIsBroke(false);
    }

    public override HintController.ActionType[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (handObject && handObject.objectSO == repairItem)
        {
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.HOLD_USE };
        }

        return new HintController.ActionType[] { HintController.ActionType.NONE };
    }
}
