using UnityEngine;

public class VendingMachine : InteractableObject
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject;
    }

    public override void Grab(ObjectHolder _objectHolder)
    {
        InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
        _objectHolder.ChangeObjectInHand(boxObject);
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", true);
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }

    public override void Release(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        return new HintController.Hint[]
  {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
  };
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
}
