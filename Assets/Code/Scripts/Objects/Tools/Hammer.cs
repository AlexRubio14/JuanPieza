using UnityEngine;

public class Hammer : Tool
{
    public override void Use(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if (!nearObject || nearObject is not RepairObject)
            return;

        (nearObject as RepairObject).AddPlayer(_objectHolder);
    }

    public override void StopUse(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if (!nearObject || nearObject is not RepairObject)
            return;

        (nearObject as RepairObject).RemovePlayer(_objectHolder);
    }
}
