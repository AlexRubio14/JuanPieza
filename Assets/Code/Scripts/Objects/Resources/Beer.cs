public class Beer : Resource, ICatapultAmmo
{

    public override void Interact(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Interact(_objectHolder);
        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player

        _objectHolder.RemoveItemFromHand();
        PlayerController controller = _objectHolder.GetComponentInParent<PlayerController>();
        controller.animator.SetBool("Pick", false);
        if (controller.stateMachine.currentState != controller.stateMachine.drunkState)
            controller.stateMachine.ChangeState(controller.stateMachine.drunkState);
        else
            controller.stateMachine.drunkState.DrinkBeer();
        Destroy(gameObject);
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
                new HintController.Hint(HintController.ActionType.USE, "drink")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
