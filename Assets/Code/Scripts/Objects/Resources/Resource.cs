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
        
        selectedVisual.Hide();
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject;
    }

    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (!handObject)
        {
            return HintController.ActionType.INTERACT;
        }
        return HintController.ActionType.NONE;
    }
}
