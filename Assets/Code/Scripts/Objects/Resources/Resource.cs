using UnityEngine;

public abstract class Resource : InteractableObject
{
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (_objectHolder.GetHasObjectPicked())
        {
            DropItem(_objectHolder);
            return;
        }

        PickItem(_objectHolder);
    }

    private void PickItem(ObjectHolder _objectHolder)
    {
        _objectHolder.SetInteractableObject(this);
        
        SetIsBeingUsed(true);

        transform.position = _objectHolder.GetObjectPickedPosition();
        transform.rotation = _objectHolder.transform.rotation;

        transform.SetParent(_objectHolder.transform.parent);

        _objectHolder.SetHasObjectPicked(true);

        selectedVisual.Hide();
        rb.isKinematic = true;
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.SetInteractableObject(null);

        SetIsBeingUsed(false);

        transform.SetParent(null);

        _objectHolder.SetHasObjectPicked(false);

        rb.isKinematic = false;
    }
}
