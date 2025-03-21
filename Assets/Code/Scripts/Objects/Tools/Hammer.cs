using UnityEngine;

public class Hammer : Tool
{
    public override void Use(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if (!nearObject)
            return;

        if(nearObject is RepairObject)
            (nearObject as RepairObject).AddPlayer(_objectHolder);
        else if (nearObject is RepairHole)
            (nearObject as RepairHole).AddPlayer(_objectHolder);
    }

    public override void StopUse(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if (!nearObject)
            return;

        if (nearObject is RepairObject)
            (nearObject as RepairObject).RemovePlayer(_objectHolder);
        else if (nearObject is RepairHole)
            (nearObject as RepairHole).RemovePlayer(_objectHolder);
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        if (handObject && handObject == this)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")

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
