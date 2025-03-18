public class Wood : Resource
{
    public override void Use(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if(!nearObject || nearObject is not RepairHole)
            return;

        (nearObject as RepairHole).AddPlayer(_objectHolder);        
    }
    public override void StopUse(ObjectHolder _objectHolder)
    {
        InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        if (!nearObject || nearObject is not RepairHole)
            return;

        (nearObject as RepairHole).RemovePlayer(_objectHolder);
    }
}
