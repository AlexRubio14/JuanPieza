public class RepairObject : Repair
{
    protected override void RepairEnded()
    {
        base.RepairEnded();
        state.SetIsBroke(false);
    }
}
