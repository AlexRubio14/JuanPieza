using UnityEngine;

public class RepairHole : Repair
{
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        ship.RemoveInteractuableObject(_objectHolder.GetNearestInteractableObject());
        Destroy(_objectHolder.gameObject);
        Destroy(gameObject);
    }
}
