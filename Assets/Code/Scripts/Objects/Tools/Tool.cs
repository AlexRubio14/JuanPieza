public abstract class Tool : InteractableObject
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

    public void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
    }
}
