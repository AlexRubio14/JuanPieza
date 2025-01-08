public class RepairObject : Repair
{
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        state.SetIsBroke(false);
    }
}
