public class Cigarrette : Resource, ICatapultAmmo
{
    public override void Release(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Release(_objectHolder);
    }
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) 
    {
        if (ShipsManager.instance != null)
            ShipsManager.instance.playerShip.Smoke();
        _objectHolder.GetComponentInParent<CigarretteController>().ActivateCigarrette();
        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));

        Destroy(gameObject);
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return _objectHolder.GetHasObjectPicked() == this;
    }
    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return new HintController.Hint[] 
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"), 
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (_objectHolder.GetHandInteractableObject() == this)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.USE, "smoke")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

}
