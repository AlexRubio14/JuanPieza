using UnityEngine;

public class RepairHole : Repair
{
    protected override void RepairEnded()
    {
        Destroy(gameObject);
    }
}
