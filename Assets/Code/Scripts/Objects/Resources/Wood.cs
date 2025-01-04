using UnityEngine;

public class Wood : Resource
{
    public override void UseItem(ObjectHolder _objectHolder)
    {
        Repair objectRepair = _objectHolder.GetInteractableObject().GetComponent<Repair>();
        if (objectRepair == null || objectRepair.GetItemNeeded() != objectSO)
            return;

        objectRepair.Interact(_objectHolder);

        Debug.Log("Uso madera");
    }
}
