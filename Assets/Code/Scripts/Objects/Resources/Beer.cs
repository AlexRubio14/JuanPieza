public class Beer : Resource, ICatapultAmmo
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
        _objectHolder.RemoveItemFromHand();
        PlayerController controller = _objectHolder.GetComponentInParent<PlayerController>();
        controller.animator.SetBool("Pick", false);
        if (controller.stateMachine.currentState != controller.stateMachine.drunkState)
            controller.stateMachine.ChangeState(controller.stateMachine.drunkState);
        else
            controller.stateMachine.drunkState.DrinkBeer();

        Destroy(gameObject);
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return _objectHolder.GetHandInteractableObject() == this;
    }
}
