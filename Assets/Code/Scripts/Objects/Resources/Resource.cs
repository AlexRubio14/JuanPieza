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
        _objectHolder.ChangeObjectInHand(this);
        
        SetIsBeingUsed(true);

        selectedVisual.Hide();
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        SetIsBeingUsed(false);
        _objectHolder.RemoveItemFromHand();
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject;
    }

}
