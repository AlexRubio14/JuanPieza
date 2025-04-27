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
        _objectHolder.playerController.animator.SetTrigger("Consume"); //Animacion de beber
        if (_objectHolder.playerController.stateMachine.currentState != _objectHolder.playerController.stateMachine.drunkState)
            _objectHolder.playerController.stateMachine.ChangeState(_objectHolder.playerController.stateMachine.drunkState);
        else
            _objectHolder.playerController.stateMachine.drunkState.DrinkBeer();

        _objectHolder.playerController.animator.SetBool("Pick", false);

        Destroy(gameObject);
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return _objectHolder.GetHandInteractableObject() == this;
    }
}
